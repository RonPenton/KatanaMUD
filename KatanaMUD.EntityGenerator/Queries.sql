use [KatanaMUD]


use [KatanaMUD]
select o.name as [Table], 
		c.name as [Column], 
		c.column_id,
		st.name as [Type], 
		c.max_length as [MaxLength], 
		c.precision as [Precision],  
		c.scale as [Scale],
		c.is_nullable as [Nullable],
		c.is_identity as [Identity],
		c.is_computed as [Computed]
from sys.objects o inner join sys.columns c on o.object_id = c.object_id
inner join sys.types st on c.user_type_id = st.user_type_id
where o.type_desc = 'USER_TABLE'
order by o.name, c.column_id


SELECT  i.name AS IndexName,
        OBJECT_NAME(ic.OBJECT_ID) AS TableName,
        COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName
FROM    sys.indexes AS i INNER JOIN 
        sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID
                                AND i.index_id = ic.index_id
WHERE   i.is_primary_key = 1

SELECT  obj.name AS FK_NAME,
    sch.name AS [schema_name],
    tab1.name AS [table],
    col1.name AS [column],
    tab2.name AS [referenced_table],
    col2.name AS [referenced_column]
FROM sys.foreign_key_columns fkc
INNER JOIN sys.objects obj
    ON obj.object_id = fkc.constraint_object_id
INNER JOIN sys.tables tab1
    ON tab1.object_id = fkc.parent_object_id
INNER JOIN sys.schemas sch
    ON tab1.schema_id = sch.schema_id
INNER JOIN sys.columns col1
    ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id
INNER JOIN sys.tables tab2
    ON tab2.object_id = fkc.referenced_object_id
INNER JOIN sys.columns col2
    ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id
order by obj.name