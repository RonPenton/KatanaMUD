/// <reference path="jquery.d.ts" />


module KMud {
    $(function () {
        $(".reset").click(evt => {
            var input = $(evt.target);
            while (down(input)) {
            }
        });
        $(".down").click(evt => {
            down($(evt.target));
        });
        $(".up").click(evt => {
            up($(evt.target));
        });
        $(".max").click(evt => {
            var input = $(evt.target);
            while (up(input)) {
            }
        });
		$(".edit").focus(evt => {
			var row = new Row($(evt.target).closest("div"));
			(<any>row.input[0]).previousValue = row.val();
		}).change(evt => {
			var row = new Row($(evt.target).closest("div"));

			var start: number = (<any>row.input[0]).previousValue;
			var end = row.val();

			var points = row.cpsCost(start, end);
			if (points == NaN || points > cps()) {
				row.val(start);
			}

			cps(points);
		});
    });

    function down(input: JQuery): boolean {
        var row = input.closest("div");
		var stats = new Row(row);

        if (stats.val() < stats.current)
            return false;

		var newVal = stats.val() - 1;
        var points = stats.cpsPerPoint(newVal + 1);
		stats.val(newVal);
		cps(points);

        return true;
    }

    function up(input: JQuery): boolean {
        var row = input.closest("div");
		var stats = new Row(row);
        var newVal = stats.val() + 1;

		var points = stats.cpsPerPoint(newVal);
        if (points > cps() || newVal > stats.max)
            return false;

		stats.val(newVal);
		cps(-points);
        return true;
    }

	function cps(val?: number) {
		if (val !== undefined) {
			$("#cps").val(String(parseInt($("#cps").val()) + val));
		}
		return parseInt($("#cps").val());
	}



	class Row {
		constructor(row: JQuery) {
			this.initial = parseInt(row.find(".initial").val());
			this.current = parseInt(row.find(".current").val());
			this.max = parseInt(row.find(".max").val());
			this.input = row.find("input[type='text']");
		}

		public initial: number;
		public current: number;
		public max: number;
		public input: JQuery;

		public val(points?: number): number {
			if (points !== undefined) {
				this.input.val(String(points));
			}

			return parseInt(this.input.val());
		}

		public cpsPerPoint(point: number): number {
			var difference = point - this.initial;
			if (difference < 0)
				throw new Error();

			// 0 -> 0
			// 1-10 -> 1
			// 11-20 -> 2
			// 21->30 -> 3
			// etc

			return Math.floor((difference + 9) / 10);
		}

		public cpsCost(start: number, end: number): number {
			if (end < this.current || end > this.max)
				return NaN;

			var cost: number = 0;
			while (end < start) {
				cost -= this.cpsPerPoint(start);
				start--;
			}
			while (end > start) {
				cost += this.cpsPerPoint(start + 1);
				start++;
			}

			return cost;
		}
	}
}
