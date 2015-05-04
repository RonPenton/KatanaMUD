module KMud {

    export interface Func0<R> { (): R; }
    export interface Func1<P, R> { (parameter: P): R; }
    export interface Func2<P1, P2, R> { (parameter1: P1, parameter2: P2): R; }
    export interface Func3<P1, P2, P3, R> { (parameter1: P1, parameter2: P2, parameter3: P3): R; }
    export interface Func4<P1, P2, P3, P4, R> { (parameter1: P1, parameter2: P2, parameter3: P3, parameter4: P4): R; }
    export interface Action0 { (): void; }
    export interface Action1<P> { (parameter: P): void; }
    export interface Action2<P1, P2> { (parameter1: P1, parameter2: P2): void; }
    export interface Action3<P1, P2, P3> { (parameter1: P1, parameter2: P2, parameter3: P3): void; }
    export interface Action4<P1, P2, P3, P4> { (parameter1: P1, parameter2: P2, parameter3: P3, parameter4: P4): void; }

    export interface Dictionary<T> { [index: string]: T }

    export class KeyPair<K, V> {
        constructor(public key?: K, public value?: V) {
        }
    } 

    export function IsString(obj: any): boolean {
        return typeof obj === 'string' || obj instanceof String;
    }
    export function IsDate(obj: any): boolean {
        return obj && typeof obj.getMonth === 'function';
    }
    export function IsDecimal(obj: any): boolean {
        if (!$.isNumeric(obj))
            return false;   // not even a number.
        return obj % 1 != 0;
    }
    export function pushRange<T>(array: T[], items: T[]) {
        for (var i = 0; i < items.length; i++) {
            array.push(items[i]);
        }
    }


    export interface ArrayLikeObject<T> {
        length: number;
        [index: number]: T;
    }

    /**
     * Creates a new Linq container.
     */
    export function Linq<T>(array: ArrayLikeObject<T>): LinqContainer<T> {
        return new LinqContainer(array);
    }
    /**
     * Creates a new Linq container for a string table.
     */
    export function LinqST<T>(table: { [index: string]: T }): LinqContainer<KeyPair<string, T>> {
        return new LinqContainer(Object.keys(table).map(x=> new KeyPair(x, table[x])));
    }

    /**
     * A class with methods that are similar to the .NET LINQ library. Note that these do not support deferred iteration and are executed immediately.
     */
    export class LinqContainer<T> {
        constructor(public values: ArrayLikeObject<T>) { }

        /**
         * Retrieves the first value that matches the given predicate, or returns null if it is not found.
         */
        public first(predicate?: Func1<T, boolean>): T {
            if (!predicate)
                return this.values[0];
            for (var i = 0; i < this.values.length; i++) {
                if (predicate(this.values[i]))
                    return this.values[i];
            }
            return null;
        }

        /**
         * Retrieves the last value that matches the given predicate, or returns null if it is not found.
         */
        public last(predicate?: Func1<T, boolean>): T {
            if (!predicate)
                return this.values[this.values.length - 1];
            for (var i = this.values.length - 1; i >= 0; i--) {
                if (predicate(this.values[i]))
                    return this.values[i];
            }
            return null;
        }

        /**
         * Scans through an array and returns only distinct values. 
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * Complexity is O(n^2) due to the lack of a generalized set data structure in Javascript. Use the distinctByX
         * methods to get O(n) complexity.
         */
        public distinct(predicate?: Func2<T, T, boolean>): LinqContainer<T> {
            if (predicate == undefined) {
                predicate = (x, y) => x == y;
            }
            var duplicateIndexes: { [index: number]: boolean } = {};
            var output: T[] = [];
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
        }

        /**
         * Scans through an array and returns only distinct values. 
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * O(n) complexity.
         */
        public distinctByStr(predicate: Func1<T, string>): LinqContainer<T> {
            var keys: { [index: string]: boolean } = {};
            var output: T[] = [];
            for (var i = 0; i < this.values.length; i++) {
                var key = predicate(this.values[i]);
                if (keys[key] === true)
                    continue;
                output.push(this.values[i]);
                keys[key] = true;
            }
            return new LinqContainer(output);
        }

        /**
         * Scans through an array and returns only distinct values. 
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * O(n) complexity.
         */
        public distinctByNum(predicate: Func1<T, number>): LinqContainer<T> {
            var keys: { [index: number]: boolean } = {};
            var output: T[] = [];
            for (var i = 0; i < this.values.length; i++) {
                var key = predicate(this.values[i]);
                if (keys[key] === true)
                    continue;
                output.push(this.values[i]);
                keys[key] = true;
            }
            return new LinqContainer(output);
        }

        /**
         * Scans through an array and returns only distinct values. 
         * The values are ordered such that the first instance is preserved, and any additional instances are discarded.
         * O(n) complexity.
         */
        public distinctByHash(predicate: Func1<T, IHashable>): LinqContainer<T> {
            return this.distinctByNum(t => predicate(t).getHashCode());
        }

        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        public orderBy(picker: Func1<T, any>, thenBy?: Func1<T, any>[]): LinqContainer<T> {
            return this._orderBy(picker, thenBy, LinqContainer._ascendingCompare);
        }

        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        public orderByStable(picker: Func1<T, any>): LinqContainer<T> {
            return this._orderBy(picker, [], LinqContainer._ascendingCompare);
        }

        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        public orderByDescending(picker: Func1<T, any>, thenBy?: Func1<T, any>[]): LinqContainer<T> {
            return this._orderBy(picker, thenBy, LinqContainer._descendingCompare);
        }

        /**
         * Orders an array by the given value picker. Values chosen by the picker must support the < and > operators.
         */
        public orderByDescendingStable(picker: Func1<T, any>): LinqContainer<T> {
            return this._orderBy(picker, [], LinqContainer._descendingCompare);
        }

        /**
         * The base orderby method.
         */
        private _orderBy(picker: Func1<T, any>, thenBy: Func1<T, any>[], comparer: Func2<any, any, number>): LinqContainer<T> {
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
        }

        /**
         * Returns true if any items in the array evaluate to true using the predicate function.
         */
        public areAny(predicate: Func1<T, boolean>): boolean {
            for (var i = 0; i < this.values.length; i++) {
                if (predicate(this.values[i]))
                    return true;
            }
            return false;
        }

        /**
        * Returns true if all items in the array evaluate to true using the predicate function.
        */
        public all(predicate: Func1<T, boolean>): boolean {
            for (var i = 0; i < this.values.length; i++) {
                if (predicate(this.values[i]) === false)
                    return false;
            }
            return true;
        }

        /**
         * Performs a left join on two arrays, returning only the records in the first array which match the predicate.
         *   * - Technically a left join should return the records in the second array as well, but for optimization purposes
         *       that step has been left out because this is more useful in this context.
         */
        public partialLeftJoin<U>(other: U[], predicate: Func2<T, U, boolean>): LinqContainer<T> {
            return new LinqContainer(this.toArray().filter(x => Linq(other).areAny(y => predicate(x, y))));
        }

        /**
         * Filters an array for items which match the predicate.
         */
        public where(predicate: Func1<T, boolean>): LinqContainer<T> {
            return new LinqContainer(this.toArray().filter(predicate));
        }

        /**
         * Maps the values in the array to a different value.
         */
        public select<U>(predicate: Func1<T, U>): LinqContainer<U> {
            return new LinqContainer(this.toArray().map(predicate));
        }

        /**
         * Returns the first X elements.
         */
        public take(amount: number): LinqContainer<T> {
            return Linq(this.toArray().slice(0, amount));
        }

        /**
         * Returns the elements of the array after the given index.
         */
        public skip(amount: number): LinqContainer<T> {
            return Linq(this.toArray().slice(amount));
        }

        /**
         * Returns the union of two arrays.
         */
        public union(other: T[]): LinqContainer<T> {
            return Linq(this.toArray().concat(other));
        }

        /**
         * Returns the union of N arrays.
         */
        public static multiUnion<T>(containers: LinqContainer<ArrayLikeObject<T>>): LinqContainer<T> {
            var array = containers.toArray();
            var running: T[] = [];
            for (var i = 0; i < array.length; i++) {
                var x = array[i];
                x
                running = running.concat(Linq(array[i]).toArray());
            }
            return new LinqContainer(running);
        }

        /**
         * Returns the max value found in the collection.
         */
        public max<U>(picker: Func1<T, U>): U {
            return picker(this.theMax(picker));
        }

        /**
         * Returns the item that contains the max value found in the collection.
         */
        public theMax<U>(picker: Func1<T, U>): T {
            return this._minOrMax(picker,(x, y) => x > y);
        }

        /**
         * Returns the min value found in the collection.
         */
        public min<U>(picker: Func1<T, U>): U {
            return picker(this.theMin(picker));
        }

        /**
         * Returns the item that contains the min value found in the collection.
         */
        public theMin<U>(picker: Func1<T, U>): T {
            return this._minOrMax(picker,(x, y) => x < y);
        }

        /**
         * Helper function used to select the min or max of an array.
         */
        private _minOrMax<U>(picker: Func1<T, U>, comp: Func2<U, U, boolean>): T {
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
        }

        /**
         * Performs a for-each loop on the array.
         */
        public forEach(action: Action2<T, number>): void {
            for (var i = 0; i < this.values.length; i++)
                action(this.values[i], i);
        }

        /**
         * Makes sure that the given object is an array before returning it. 
         * Otherwise, it converts the item to an array.
         */
        public toArray(): T[] {
            if ($.isArray(this.values))
                return <T[]>this.values;
            var array: T[] = [];
            this.forEach(x => array.push(x));
            return array;
        }

        /**
         * Clones the object into a new array.
         */
        public clone(): T[] {
            if ($.isArray(this.values))
                return (<T[]>this.values).slice(0);
            var array: T[] = [];
            this.forEach(x => array.push(x));
            return array;
        }

        static _pickerCompare<T>(picker: Func1<T, any>, compare: Func2<any, any, number>): Func2<T, T, number> {
            return (a: T, b: T): number => {
                var t = picker(a);
                var u = picker(b);
                return compare(t, u);
            }
        }

        static _ascendingCompare<T>(a: T, b: T): number {
            // special case. The < and > operators do not factor in case, so if we're comparing strings, go with a locale compare.
            if (IsString(a) && IsString(b))
                return (<string><any>a).localeCompare(<string><any>b);

            if (a > b)
                return 1;
            if (a < b)
                return -1;
            return 0;
        }

        static _descendingCompare<T>(a: T, b: T): number {
            // special case. The < and > operators do not factor in case, so if we're comparing strings, go with a locale compare.
            if (IsString(a) && IsString(b))
                return -(<string><any>a).localeCompare(<string><any>b);

            if (a < b)
                return 1;
            if (a > b)
                return -1;
            return 0;
        }

        /**
        * Groups a container using a string key. Groups should be assumed to be unordered. O(n) performance. 
        */
        public groupBy(picker: Func1<T, string> | Func1<T, number>): LinqContainer<Grouping<T, any>> {
            var groups: any = {};
            this.forEach(x => {
                var key = <any>picker(x);
                if (groups[key] === undefined) {
                    groups[key] = []
                }
                groups[key].push(x);
            });
            var output: Grouping<T, any>[] = [];
            var keys = Object.keys(groups);
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                output.push(new Grouping<T, any>(key, groups[key]));
            }

            return new LinqContainer(output);
        }
    }

    /**
     * Defines an interface for objects which support returning hash codes. A hash code is assumed to be a completely unique number
     * representing the item.
     */
    export interface IHashable {
        getHashCode(): number;
    }


    /**
     * Provides methods to perform a merge sort. While typically slower than a quick sort, it is a stable sort, which is sometimes necessary in some conditions.
     */
    module MergeSort {
        export function run<T>(values: T[], compareFn: (a: T, b: T) => number) {
            if (!values || values.length == 0)
                return;
            _mergeSort(values, values.slice(0), values.length, compareFn);
        }

        function _mergeSort<T>(values: T[], temp: T[], length: number, compareFn: (a: T, b: T) => number) {
            if (length == 1) return;
            var m = Math.floor(length / 2);
            var tmp_l = temp.slice(0, m);
            var tmp_r = temp.slice(m);
            _mergeSort(tmp_l, values.slice(0, m), m, compareFn);
            _mergeSort(tmp_r, values.slice(m), length - m, compareFn);
            _merge(tmp_l, tmp_r, values, compareFn);
        }


        function _merge<T>(left: T[], right: T[], values: T[], compareFn: (a: T, b: T) => number) {
            var a = 0;
            while (left.length && right.length) {
                values[a++] = compareFn(right[0], left[0]) < 0 ? right.shift() : left.shift();
            }
            while (left.length) values[a++] = left.shift();
            while (right.length) values[a++] = right.shift();
        }
    }

    export class Grouping<T, K> extends LinqContainer<T> {
        constructor(public key: K, values: ArrayLikeObject<T>) {
            super(values);
        }
    }
}