using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.EntityGenerator
{
    class Program
    {
        static string className = "GameEntities";
        static string namespaceName = "KatanaMUD.Models";
        static string location = "..\\..\\..\\src\\KatanaMUD\\Models\\";



        static void Main(string[] args)
        {
            AddTypeOverride("Item", "EquippedSlot", "EquipmentSlot?");
            AddTypeOverride("ItemTemplate", "WeaponType", "WeaponType?");
            AddTypeOverride("ItemTemplate", "EquipType", "EquipmentSlot?");

            AddAccessOverride("Item", "JSONStats", "private");
            AddAccessOverride("Item", "JSONAttributes", "private");

            AddAccessOverride("Actor", "JSONStats", "private");
            AddAccessOverride("Actor", "JSONAttributes", "private");

            string connectionString = "Data Source=(local);Initial Catalog=KatanaMUD;Integrated Security=true";
            List<ColumnMetadata> columns = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    columns = GetColumns(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            GenerateFiles(columns);
        }

        private static void GenerateFiles(List<ColumnMetadata> columns)
        {
            var tables = columns.Select(x => x.Table).Distinct();

            var normalTables = new List<Table>();
            var linkTables = new List<Table>();

            foreach (var tableName in tables)
            {
                var table = new Table(tableName);
                table.Columns = columns.Where(x => x.Table == tableName).ToList();
                var keys = table.Columns.Where(x => x.PrimaryKey);

                if (keys.Count() == 1)
                {
                    normalTables.Add(table);
                }
                else if (keys.Count() == 2 && table.Columns.Count == 2 && !table.Columns.Any(x => x.ForeignKeyTable == null) && !table.Columns.Any(x => x.ForeignKeyColumn == null))
                {
                    linkTables.Add(table);
                }
                else
                {
                    throw new InvalidOperationException("Invalid Table Type: " + tableName);
                }
            }

            foreach (var table in linkTables)
            {
                var first = normalTables.Single(x => x.Name == table.Columns.First().ForeignKeyTable);
                var second = normalTables.Single(x => x.Name == table.Columns.Last().ForeignKeyTable);

                first.LinkEntities.Add(table);
                second.LinkEntities.Add(table);

                table.FirstLink = first;
                table.SecondLink = second;
            }

            foreach (var column in columns.Where(x => x.ForeignKeyTable != null))
            {
                var parent = normalTables.SingleOrDefault(x => x.Name == column.ForeignKeyTable);
                var child = normalTables.SingleOrDefault(x => x.Name == column.Table);

                if (parent == null || child == null)
                    continue;
                parent.Children.Add(child);
                child.Relationships.Add(new Relationship() { Column = column, ParentTable = parent });
            }

            StringBuilder context = new StringBuilder();

            context.AppendFormat(@"using Spam;
using System;
using System.Data.SqlClient;

namespace {0}
{{
    public partial class {1} : Spam.EntityContext
    {{
        public {1}(string connectionString) : base(connectionString)
        {{
", namespaceName, className);

            foreach (var table in normalTables)
            {
                context.AppendFormat("            {0} = new EntityContainer<{1}, {2}>(this);\r\n", table.PluralName, table.Name, table.PrimaryKey.TypeName);
            }

            context.Append("        }\r\n\r\n");

            foreach (var table in normalTables)
            {
                context.AppendFormat("        public EntityContainer<{0}, {1}> {2} {{ get; private set; }}\r\n", table.Name, table.PrimaryKey.TypeName, table.PluralName);
            }

            foreach (var link in linkTables)
            {
                context.AppendFormat("        internal LinkEntityContainer<{0}, {1}, {2}, {3}> {4} = new LinkEntityContainer<{0}, {1}, {2}, {3}>(\"{5}\", \"{6}\", \"{7}\");\r\n",
                    link.FirstLink.Name, link.SecondLink.Name, link.FirstLink.PrimaryKey.TypeName, link.SecondLink.PrimaryKey.TypeName,
                    link.PluralName, link.Name, link.Columns.First().Column, link.Columns.Last().Column);
            }

            context.Append(@"        protected override void LoadMetaData()
        {
            EntityMetadata meta;

");

            foreach (var table in normalTables)
            {
                context.AppendFormat("            meta = new EntityMetadata() {{ EntityType = typeof({0}), Container = {1} }};\r\n", table.Name, table.PluralName);

                foreach (var relationship in table.Relationships)
                {
                    context.AppendFormat("            meta.Relationships.Add(new EntityRelationship((IEntity e) => (({0})e).{1}));\r\n", table.Name, relationship.Column.ForeignKeyTable);
                }

                context.AppendFormat(@"            meta.GenerateInsertCommand = (SqlCommand c, IEntity e) => {0}.GenerateInsertCommand(c, ({0})e);
            meta.GenerateUpdateCommand = (SqlCommand c, IEntity e) => {0}.GenerateUpdateCommand(c, ({0})e);
            meta.GenerateDeleteCommand = (SqlCommand c, IEntity e) => {0}.GenerateDeleteCommand(c, ({0})e);
            EntityTypes.Add(meta);", table.Name);
                context.Append("\r\n\r\n");
            }

            foreach (var link in linkTables)
            {
                context.AppendFormat("            this.LinkTypes.Add({0});\r\n", link.PluralName);
            }

            context.Append(@"        }

        protected override void LoadAllData(SqlConnection connection)
        {
");

            foreach (var table in normalTables)
            {
                context.AppendFormat("            LoadData(connection, {0}, \"{1}\", {1}.Load);\r\n", table.PluralName, table.Name);
            }

            foreach (var link in linkTables)
            {
                context.AppendFormat("            {0}.Load(connection);\r\n", link.PluralName);
            }

            context.Append(@"        }
    }
}");


            var path = Path.Combine(location, "GameEntities.cs");
            File.WriteAllText(path, context.ToString());


            foreach (var table in normalTables)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(@"using Spam;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace {0}
{{
", namespaceName);
                builder.AppendFormat("    public partial class {0} : Entity<{1}>\r\n    {{\r\n", table.Name, table.PrimaryKey.TypeName);
                builder.Append("        partial void OnConstruct();\r\n");
                builder.AppendFormat("        public override {0} Key {{ get {{ return {1}; }} set {{ {1} = value; }} }}\r\n", table.PrimaryKey.TypeName, table.PrimaryKey.Column);
                builder.AppendFormat("        private {0} Context => ({0})__context;\r\n", className);

                foreach (var column in table.NormalColumns)
                {
                    builder.AppendFormat("        private {0} _{1};\r\n", column.TypeName, column.Column);
                }

                foreach (var relationship in table.Relationships)
                {
                    builder.AppendFormat("        private {0} _{1};\r\n", relationship.Column.TypeName, relationship.Column.Column);
                    builder.AppendFormat("        private {0} _{0};\r\n", relationship.Column.ForeignKeyTable);
                }

                builder.AppendFormat("\r\n        public {0}()\r\n        {{\r\n", table.Name);
                builder.Append("            OnConstruct();\r\n");
                foreach (var column in table.JsonColumns)
                {
                    builder.AppendFormat("            {0} = new JsonContainer(this);\r\n", column.Column);
                }
                foreach (var child in table.Children)
                {
                    builder.AppendFormat("            {0} = new ParentChildRelationshipContainer<{1}, {2}, {3}>(this, child => child.{1}, (child, parent) => child.{1}= parent);\r\n", child.PluralName, table.Name, child.Name, child.PrimaryKey.TypeName);
                }
                foreach (var linkTable in table.LinkEntities)
                {
                    var link = linkTable.FirstLink;
                    if (link.Name == table.Name)
                        link = linkTable.SecondLink;

                    builder.AppendFormat(@"            {0} = new ObservableHashSet<{1}>();
            {0}.ItemsAdded += {0}_ItemsAdded;
            {0}.ItemsRemoved += {0}_ItemsRemoved;
", link.PluralName, link.Name);
                }
                builder.Append("        }\r\n\r\n");

                foreach (var column in table.NormalColumns)
                {
                    var alteredType = GetTypeOverride(table.Name, column.Column);
                    var access = GetAccessOverride(table.Name, column.Column);
                    if (alteredType == null)
                    {
                        builder.AppendFormat("        {2} {0} {1} {{ get {{ return _{1}; }} set {{ _{1} = value; this.Changed(); }} }}\r\n", column.TypeName, column.Column, access);
                    }
                    else
                    {
                        builder.AppendFormat("        {2} {0} {1} {{ get {{ return ({0})_{1}; }} set {{ _{1} = ({3})value; this.Changed(); }} }}\r\n", alteredType, column.Column, access, column.TypeName);
                    }
                }

                foreach (var column in table.JsonColumns)
                {
                    var access = GetAccessOverride(table.Name, column.Column);
                    string setAccess = "private ";
                    if (access == "private")
                        setAccess = "";
                    builder.AppendFormat("        {1} JsonContainer {0} {{ get; {2}set; }}\r\n", column.Column, access, setAccess);
                }

                foreach (var relationship in table.Relationships)
                {
                    builder.AppendFormat("        partial void On{0}Changing({0} oldValue, {0} newValue);\r\n", relationship.Column.ForeignKeyTable);
                    builder.AppendFormat("        public {0} {0} {{\r\n", relationship.Column.ForeignKeyTable);
                    builder.AppendFormat("            get {{ return _{0}; }}\r\n", relationship.Column.ForeignKeyTable);
                    builder.AppendFormat(@"            set
            {{
                    On{0}Changing(_{0}, value);
                    ChangeParent(value, ref _{0}, 
                    ({0} parent, {1} child) => parent.{2}.Remove(child), 
                    ({0} parent, {1} child) => parent.{2}.Add(child));
            }}
        }}

", relationship.Column.ForeignKeyTable, table.Name, table.PluralName);
                }

                foreach (var child in table.Children)
                {
                    builder.AppendFormat("        public ICollection<{0}> {1} {{ get; private set; }}\r\n",
                        child.Name, child.PluralName);
                }
                foreach (var link in table.LinkEntities)
                {
                    var other = link.FirstLink;
                    if (other.Name == table.Name)
                        other = link.SecondLink;
                    builder.AppendFormat("        public ObservableHashSet<{0}> {1} {{ get; private set; }}\r\n", other.Name, other.PluralName);
                }



                builder.AppendFormat(@"        public static {0} Load(SqlDataReader reader)
        {{
", table.Name);

                builder.AppendFormat("            var entity = new {0}();\r\n", table.Name);

                var i = 0;
                foreach (var column in table.Columns)
                {
                    if (!column.Column.StartsWith("JSON", StringComparison.InvariantCultureIgnoreCase))
                    {
                        builder.AppendFormat("            entity._{0} = reader.Get{1}({2});\r\n", column.Column, GetSqlName(column), i);
                    }
                    else
                    {
                        builder.AppendFormat(@"            entity.{0} = new JsonContainer(entity);
            entity.{0}.FromJson(reader.GetSafeString({1}));
", column.Column, i);
                    }

                    i++;
                }

                builder.AppendFormat(@"            return entity;
        }}

        public override void LoadRelationships()
        {{
", table.Name);

                foreach (var relationship in table.Relationships)
                {
                    builder.AppendFormat("            {0} = Context.{1}.Single{3}(x => x.Id == _{2});\r\n",
                        relationship.Column.ForeignKeyTable, relationship.ParentTable.PluralName, relationship.Column.Column,
                        relationship.Column.Nullable ? "OrDefault" : "");
                }

                foreach (var link in table.LinkEntities)
                {
                    var firstItem = "Item2";
                    var secondItem = "Item1";
                    var first = link.FirstLink;
                    var second = link.SecondLink;
                    if (second.Name == table.Name)
                    {
                        firstItem = "Item1";
                        secondItem = "Item2";
                        first = link.SecondLink;
                        second = link.FirstLink;
                    }

                    builder.AppendFormat("            {0}.AddRange(Context.{1}.Where(x => x.{2} == this.{3}).Select(x => Context.{0}.Single(y => y.{5} == x.{4})), true);\r\n",
                        second.PluralName, link.PluralName, secondItem, table.PrimaryKey.Column, firstItem, second.PrimaryKey.Column);
                }



                builder.AppendFormat(@"        }}

        private static void AddSqlParameters(SqlCommand c, {0} e)
        {{
            c.Parameters.Clear();
", table.Name);

                foreach (var column in table.Columns)
                {
                    if (column.Column.StartsWith("JSON", StringComparison.InvariantCultureIgnoreCase))
                    {
                        builder.AppendFormat("            c.Parameters.AddWithValue(\"@{0}\", e.{1}.ToJson());\r\n", column.Column, column.Column);
                    }
                    else if (column.ForeignKeyTable != null)
                    {
                        builder.AppendFormat("            c.Parameters.AddWithValue(\"@{0}\", (object)e.{1}?.Id ?? DBNull.Value);\r\n", column.Column, column.ForeignKeyTable);
                    }
                    else
                    {
                        builder.AppendFormat("            c.Parameters.AddWithValue(\"@{0}\", (object)e.{0} ?? DBNull.Value);\r\n", column.Column);
                    }
                }

                builder.AppendFormat(@"        }}

        public static void GenerateInsertCommand(SqlCommand c, {0} e)
        {{
            c.CommandText = @""INSERT INTO [{0}](", table.Name);

                builder.Append(String.Join(", ", table.Columns.Select(x => "[" + x.Column + "]")) + ")\r\n");
                builder.Append("                              VALUES (");
                builder.Append(String.Join(", ", table.Columns.Select(x => "@" + x.Column)));
                builder.Append(@")"";
            AddSqlParameters(c, e);
        }
");
                builder.AppendFormat(@"
        public static void GenerateUpdateCommand(SqlCommand c, {0} e)
        {{
            c.CommandText = @""UPDATE [{0}] SET ", table.Name);

                builder.Append(String.Join(", ", table.Columns.Select(x => "[" + x.Column + "] = @" + x.Column)));
                builder.Append(" WHERE [Id] = @Id\";\r\n");

                builder.AppendFormat(@"            AddSqlParameters(c, e);
        }}

        public static void GenerateDeleteCommand(SqlCommand c, {0} e)
        {{
            c.CommandText = @""DELETE FROM[{0}] WHERE[Id] = @Id"";
            c.Parameters.Clear();
            c.Parameters.AddWithValue(""@Id"", e.{1});
        }}
", table.Name, table.PrimaryKey.Column);

                foreach (var link in table.LinkEntities)
                {
                    var item1Key = "item.Key";
                    var item2Key = "this.Key";
                    var current = link.SecondLink;
                    var other = link.FirstLink;
                    if (other.Name == table.Name)
                    {
                        other = link.SecondLink;
                        current = link.FirstLink;
                        item1Key = "this.Key";
                        item2Key = "item.Key";
                    }

                    builder.AppendFormat(@"        private void {0}_ItemsAdded(object sender, CollectionChangedEventArgs<{1}> e)
        {{
            foreach (var item in e.Items)
            {{
                item.{2}.Add(this, true);
                Context.{3}.Link({4}, {5}, false);
            }}
        }}
        private void {0}_ItemsRemoved(object sender, CollectionChangedEventArgs<{1}> e)
        {{
            foreach (var item in e.Items)
            {{
                item.{2}.Remove(this, true);
                Context.{3}.Unlink({4}, {5});
            }}
        }}

", other.PluralName, other.Name, current.PluralName, link.PluralName, item1Key, item2Key);
                }

                builder.Append(@"    }
}");

                var epath = Path.Combine(location, table.Name + ".cs");
                File.WriteAllText(epath, builder.ToString());
            }
        }

        private static string GetSqlName(ColumnMetadata column)
        {
            var prefix = "";
            if (column.Nullable)
            {
                prefix = "Safe";
            }

            if (column.DataType == "int")
                return prefix + "Int32";
            if (column.DataType == "uniqueidentifier")
                return prefix + "Guid";
            if (column.DataType == "nvarchar")
                return prefix + "String";
            if (column.DataType == "datetime")
                return prefix + "DateTime";
            if (column.DataType == "datetimeoffset")
                return prefix + "DateTimeOffset";
            if (column.DataType == "bit")
                return prefix + "Boolean";
            if (column.DataType == "bigint")
                return prefix + "Int64";
            if (column.DataType == "float")
                return prefix + "Double";

            throw new InvalidOperationException("Datatype not supported: " + column.TypeName.ToString());
        }


        private static List<ColumnMetadata> GetColumns(SqlConnection connection)
        {
            string query = @"use [KatanaMUD]
select o.name as [Table], 
        c.name as [Column], 
        c.column_id,
        st.name as [Type], 
        c.max_length as [MaxLength], 
        c.precision as [Precision],  
        c.scale as [Scale],
        c.is_nullable as [Nullable],
        c.is_identity as [Identity],
        c.is_computed as [Computed],
        i.is_primary_key as [Primary],
        fkt.name as [ForeignKeyTable],
        refc.name as [ForeignKeyColumn]
from sys.objects o inner join sys.columns c on o.object_id = c.object_id
inner join sys.types st on c.user_type_id = st.user_type_id
left join sys.index_columns ic on ic.object_id = o.object_id and ic.column_id = c.column_id
left join sys.indexes i on i.object_id = ic.object_id
left join sys.foreign_key_columns fkc on fkc.parent_object_id = o.object_id and fkc.parent_column_id = c.column_id
left join sys.tables fkt on fkc.referenced_object_id = fkt.object_id
left join sys.columns refc on fkc.referenced_object_id = refc.object_id and fkc.referenced_column_id = refc.column_id
where o.type_desc = 'USER_TABLE' and o.name != 'sysdiagrams'
order by o.name, c.column_id";

            SqlCommand command = new SqlCommand(query, connection);
            List<ColumnMetadata> list = new List<ColumnMetadata>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new ColumnMetadata(reader));
                }
            }
            return list;
        }

        static Dictionary<string, Dictionary<string, string>> _typeOverrides = new Dictionary<string, Dictionary<string, string>>();
        static Dictionary<string, Dictionary<string, string>> _accessOverrides = new Dictionary<string, Dictionary<string, string>>();


        static void AddTypeOverride(string table, string column, string typeName)
        {
            Dictionary<string, string> td;
            if (!_typeOverrides.TryGetValue(table, out td))
            {
                _typeOverrides[table] = td = new Dictionary<string, string>();
            }

            td[column] = typeName;
        }

        static void AddAccessOverride(string table, string column, string accessName)
        {
            Dictionary<string, string> td;
            if (!_accessOverrides.TryGetValue(table, out td))
            {
                _accessOverrides[table] = td = new Dictionary<string, string>();
            }

            td[column] = accessName;
        }

        static string GetTypeOverride(string table, string column)
        {
            Dictionary<string, string> td;
            if (_typeOverrides.TryGetValue(table, out td))
            {
                string value;
                if (td.TryGetValue(column, out value))
                    return value;
            }

            return null;
        }

        static string GetAccessOverride(string table, string column)
        {
            Dictionary<string, string> td;
            if (_accessOverrides.TryGetValue(table, out td))
            {
                string value;
                if (td.TryGetValue(column, out value))
                    return value;
            }

            return "public";
        }
    }
}
