var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var KMud;
(function (KMud) {
    var KeyPair = (function () {
        function KeyPair(key, value) {
            this.key = key;
            this.value = value;
        }
        return KeyPair;
    })();
    KMud.KeyPair = KeyPair;
    function IsString(obj) {
        return typeof obj === 'string' || obj instanceof String;
    }
    KMud.IsString = IsString;
    function IsDate(obj) {
        return obj && typeof obj.getMonth === 'function';
    }
    KMud.IsDate = IsDate;
    function IsDecimal(obj) {
        if (!$.isNumeric(obj))
            return false; // not even a number.
        return obj % 1 != 0;
    }
    KMud.IsDecimal = IsDecimal;
    /**
     * Creates a new Linq container.
     */
    function Linq(array) {
        return new LinqContainer(array);
    }
    KMud.Linq = Linq;
    /**
     * Creates a new Linq container for a string table.
     */
    function LinqST(table) {
        return new LinqContainer(Object.keys(table).map(function (x) { return new KeyPair(x, table[x]); }));
    }
    KMud.LinqST = LinqST;
    /**
     * A class with methods that are similar to the .NET LINQ library. Note that these do not support deferred iteration and are executed immediately.
     */
    var LinqContainer = (function () {
        function LinqContainer(values) {
            this.values = values;
        }
        /**
         * Retrieves the first value that matches the given predicate, or returns null if it is not found.
         */
        LinqContainer.prototype.first = function (predicate) {
            if (!predicate)
                return this.values[0];
            for (var i = 0; i < this.values.length; i++) {
                if (predicate(this.values[i]))
                    return this.values[i];
            }
            return null;
        };
        /**
         * Retrieves the last value that matches the given predicate, or returns null if it is not found.
         */
        LinqContainer.prototype.last = function (predicate) {
            if (!predicate)
                return this.values[this.values.length - 1];
            for (var i = this.values.length - 1; i >= 0; i--) {
                if (predicate(this.values[i]))
                    return this.values[i];
            }
            return null;
        };
        /**
         * Scans through an array and returns only distinct values.
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * Complexity is O(n^2) due to the lack of a generalized set data structure in Javascript. Use the distinctByX
         * methods to get O(n) complexity.
         */
        LinqContainer.prototype.distinct = function (predicate) {
            if (predicate == undefined) {
                predicate = function (x, y) { return x == y; };
            }
            var duplicateIndexes = {};
            var output = [];
            for (var i = 0; i < this.values.length; i++) {
                if (duplicateIndexes[i] === true)
                    continue;
                output.push(this.values[i]);
                for (var j = i + 1; j < this.values.length; j++) {
                    if (predicate(this.values[i], this.values[j]) === true)
                        duplicateIndexes[j] = true;
                }
            }
            return new LinqContainer(output);
        };
        /**
         * Scans through an array and returns only distinct values.
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * O(n) complexity.
         */
        LinqContainer.prototype.distinctByStr = function (predicate) {
            var keys = {};
            var output = [];
            for (var i = 0; i < this.values.length; i++) {
                var key = predicate(this.values[i]);
                if (keys[key] === true)
                    continue;
                output.push(this.values[i]);
                keys[key] = true;
            }
            return new LinqContainer(output);
        };
        /**
         * Scans through an array and returns only distinct values.
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * O(n) complexity.
         */
        LinqContainer.prototype.distinctByNum = function (predicate) {
            var keys = {};
            var output = [];
            for (var i = 0; i < this.values.length; i++) {
                var key = predicate(this.values[i]);
                if (keys[key] === true)
                    continue;
                output.push(this.values[i]);
                keys[key] = true;
            }
            return new LinqContainer(output);
        };
        /**
         * Scans through an array and returns only distinct values.
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * O(n) complexity.
         */
        LinqContainer.prototype.distinctByHash = function (predicate) {
            return this.distinctByNum(function (t) { return predicate(t).getHashCode(); });
        };
        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        LinqContainer.prototype.orderBy = function (picker, thenBy) {
            return this._orderBy(picker, thenBy, LinqContainer._ascendingCompare);
        };
        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        LinqContainer.prototype.orderByStable = function (picker) {
            return this._orderBy(picker, [], LinqContainer._ascendingCompare);
        };
        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        LinqContainer.prototype.orderByDescending = function (picker, thenBy) {
            return this._orderBy(picker, thenBy, LinqContainer._descendingCompare);
        };
        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        LinqContainer.prototype.orderByDescendingStable = function (picker) {
            return this._orderBy(picker, [], LinqContainer._descendingCompare);
        };
        /**
         * The base orderby method.
         */
        LinqContainer.prototype._orderBy = function (picker, thenBy, comparer) {
            var clone = this.clone();
            if (thenBy === undefined) {
                clone.sort(LinqContainer._pickerCompare(picker, comparer));
            }
            else {
                // ThenBy clause specified. Switch to a stable sort to preserve the order on multiple passes.
                thenBy.unshift(picker);
                for (var i = thenBy.length - 1; i >= 0; i--) {
                    MergeSort.run(clone, LinqContainer._pickerCompare(thenBy[i], comparer));
                }
            }
            return new LinqContainer(clone);
        };
        /**
         * Returns true if any items in the array evaluate to true using the predicate function.
         */
        LinqContainer.prototype.areAny = function (predicate) {
            for (var i = 0; i < this.values.length; i++) {
                if (predicate(this.values[i]))
                    return true;
            }
            return false;
        };
        /**
        * Returns true if all items in the array evaluate to true using the predicate function.
        */
        LinqContainer.prototype.all = function (predicate) {
            for (var i = 0; i < this.values.length; i++) {
                if (predicate(this.values[i]) === false)
                    return false;
            }
            return true;
        };
        /**
         * Performs a left join on two arrays, returning only the records in the first array which match the predicate.
         *   * - Technically a left join should return the records in the second array as well, but for optimization purposes
         *       that step has been left out because this is more useful in this context.
         */
        LinqContainer.prototype.partialLeftJoin = function (other, predicate) {
            return new LinqContainer(this.toArray().filter(function (x) { return Linq(other).areAny(function (y) { return predicate(x, y); }); }));
        };
        /**
         * Filters an array for items which match the predicate.
         */
        LinqContainer.prototype.where = function (predicate) {
            return new LinqContainer(this.toArray().filter(predicate));
        };
        /**
         * Maps the values in the array to a different value.
         */
        LinqContainer.prototype.select = function (predicate) {
            return new LinqContainer(this.toArray().map(predicate));
        };
        /**
         * Returns the first X elements.
         */
        LinqContainer.prototype.take = function (amount) {
            return Linq(this.toArray().slice(0, amount));
        };
        /**
         * Returns the elements of the array after the given index.
         */
        LinqContainer.prototype.skip = function (amount) {
            return Linq(this.toArray().slice(amount));
        };
        /**
         * Returns the union of two arrays.
         */
        LinqContainer.prototype.union = function (other) {
            return Linq(this.toArray().concat(other));
        };
        /**
         * Returns the union of N arrays.
         */
        LinqContainer.multiUnion = function (containers) {
            var array = containers.toArray();
            var running = [];
            for (var i = 0; i < array.length; i++) {
                var x = array[i];
                x;
                running = running.concat(Linq(array[i]).toArray());
            }
            return new LinqContainer(running);
        };
        /**
         * Returns the max value found in the collection.
         */
        LinqContainer.prototype.max = function (picker) {
            return picker(this.theMax(picker));
        };
        /**
         * Returns the item that contains the max value found in the collection.
         */
        LinqContainer.prototype.theMax = function (picker) {
            return this._minOrMax(picker, function (x, y) { return x > y; });
        };
        /**
         * Returns the min value found in the collection.
         */
        LinqContainer.prototype.min = function (picker) {
            return picker(this.theMin(picker));
        };
        /**
         * Returns the item that contains the min value found in the collection.
         */
        LinqContainer.prototype.theMin = function (picker) {
            return this._minOrMax(picker, function (x, y) { return x < y; });
        };
        /**
         * Helper function used to select the min or max of an array.
         */
        LinqContainer.prototype._minOrMax = function (picker, comp) {
            var selected = picker(this.values[0]);
            var index = 0;
            var last = selected;
            for (var i = 1; i < this.values.length; i++) {
                var current = picker(this.values[i]);
                if (comp(current, selected)) {
                    selected = current;
                    index = i;
                }
                last = current;
            }
            return this.values[index];
        };
        /**
         * Performs a for-each loop on the array.
         */
        LinqContainer.prototype.forEach = function (action) {
            for (var i = 0; i < this.values.length; i++)
                action(this.values[i], i);
        };
        /**
         * Makes sure that the given object is an array before returning it.
         * Otherwise, it converts the item to an array.
         */
        LinqContainer.prototype.toArray = function () {
            if ($.isArray(this.values))
                return this.values;
            var array = [];
            this.forEach(function (x) { return array.push(x); });
            return array;
        };
        /**
         * Clones the object into a new array.
         */
        LinqContainer.prototype.clone = function () {
            if ($.isArray(this.values))
                return this.values.slice(0);
            var array = [];
            this.forEach(function (x) { return array.push(x); });
            return array;
        };
        LinqContainer._pickerCompare = function (picker, compare) {
            return function (a, b) {
                var t = picker(a);
                var u = picker(b);
                return compare(t, u);
            };
        };
        LinqContainer._ascendingCompare = function (a, b) {
            // special case. The < and > operators do not factor in case, so if we're comparing strings, go with a locale compare.
            if (IsString(a) && IsString(b))
                return a.localeCompare(b);
            if (a > b)
                return 1;
            if (a < b)
                return -1;
            return 0;
        };
        LinqContainer._descendingCompare = function (a, b) {
            // special case. The < and > operators do not factor in case, so if we're comparing strings, go with a locale compare.
            if (IsString(a) && IsString(b))
                return -a.localeCompare(b);
            if (a < b)
                return 1;
            if (a > b)
                return -1;
            return 0;
        };
        /**
        * Groups a container using a string key. Groups should be assumed to be unordered. O(n) performance.
        */
        LinqContainer.prototype.groupBy = function (picker) {
            var groups = {};
            this.forEach(function (x) {
                var key = picker(x);
                if (groups[key] === undefined) {
                    groups[key] = [];
                }
                groups[key].push(x);
            });
            var output = [];
            var keys = Object.keys(groups);
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                output.push(new Grouping(key, groups[key]));
            }
            return new LinqContainer(output);
        };
        return LinqContainer;
    })();
    KMud.LinqContainer = LinqContainer;
    /**
     * Provides methods to perform a merge sort. While typically slower than a quick sort, it is a stable sort, which is sometimes necessary in some conditions.
     */
    var MergeSort;
    (function (MergeSort) {
        function run(values, compareFn) {
            if (!values || values.length == 0)
                return;
            _mergeSort(values, values.slice(0), values.length, compareFn);
        }
        MergeSort.run = run;
        function _mergeSort(values, temp, length, compareFn) {
            if (length == 1)
                return;
            var m = Math.floor(length / 2);
            var tmp_l = temp.slice(0, m);
            var tmp_r = temp.slice(m);
            _mergeSort(tmp_l, values.slice(0, m), m, compareFn);
            _mergeSort(tmp_r, values.slice(m), length - m, compareFn);
            _merge(tmp_l, tmp_r, values, compareFn);
        }
        function _merge(left, right, values, compareFn) {
            var a = 0;
            while (left.length && right.length) {
                values[a++] = compareFn(right[0], left[0]) < 0 ? right.shift() : left.shift();
            }
            while (left.length)
                values[a++] = left.shift();
            while (right.length)
                values[a++] = right.shift();
        }
    })(MergeSort || (MergeSort = {}));
    var Grouping = (function (_super) {
        __extends(Grouping, _super);
        function Grouping(key, values) {
            _super.call(this, values);
            this.key = key;
        }
        return Grouping;
    })(LinqContainer);
    KMud.Grouping = Grouping;
})(KMud || (KMud = {}));
