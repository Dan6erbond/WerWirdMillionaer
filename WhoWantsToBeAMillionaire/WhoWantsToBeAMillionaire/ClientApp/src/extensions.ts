declare global {
    interface Array<T> {
        sortBy(property: string, ascending: boolean): Array<T>;
    }
}

if (!Array.prototype.hasOwnProperty('sortBy')) {
    Array.prototype.sortBy = function (property: string, ascending: boolean) {
        return this.sort(function (a, b) {
            return a[property] === b[property] ? 0 : (a[property] > b[property]) && ascending ? 1 : -1;
        });
    };
}

export {};