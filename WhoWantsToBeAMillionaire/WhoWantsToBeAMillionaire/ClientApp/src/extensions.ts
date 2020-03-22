declare global {
    interface Array<T> {
        sortBy(property: string, ascending: boolean): Array<T>;
    }

    interface Number {
        toTimeString(): string;
    }
}

if (!Array.prototype.hasOwnProperty('sortBy')) {
    Array.prototype.sortBy = function (property: string, ascending: boolean) {
        return this.sort(function (a, b) {
            return a[property] === b[property] ? 0 : (a[property] > b[property]) && ascending ? 1 : -1;
        });
    };
}

if (!Number.prototype.hasOwnProperty('toTimeString')) {
    Number.prototype.toTimeString = function () {
        const value = this.valueOf();
        
        if (value >= 60) {
            let minutes = Math.floor(value / 60);
            let seconds = value % 60;
            
            if (minutes != 1 && seconds != 1) {
                return `${minutes} minutes ${seconds} seconds`;
            } else if (minutes != 1) {
                return `${minutes} minutes ${seconds} second`;
            } else {
                return `${minutes} minute ${seconds} seconds`;
            }
        } else if (value != 1) {
            return `${value} seconds`;
        } else {
            return `${value} second`;
        }
    };
}

export {};