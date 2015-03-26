var KMud;
(function (KMud) {
    var Game = (function () {
        function Game() {
        }
        Game.prototype.connect = function (url) {
            var _this = this;
            this._socket = new WebSocket(url, "kmud");
            this._socket.onopen = function (e) { return _this.onopen(e); };
            this._socket.onclose = function (e) { return _this.onclose(e); };
            this._socket.onerror = function (e) { return _this.onerror(e); };
            this._socket.onmessage = function (e) { return _this.onmessage(e); };
        };
        Game.prototype.SendMessage = function (message) {
            if (this._socket) {
                this._socket.send(JSON.stringify(message));
            }
        };
        Game.prototype.onopen = function (e) {
        };
        Game.prototype.onclose = function (e) {
            this.addOutput(document.getElementById("Output"), "Disconnected", "error-text");
        };
        Game.prototype.onerror = function (e) {
        };
        Game.prototype.onmessage = function (e) {
            var message = JSON.parse(e.data);
            if (message.MessageName == KMud.LoginRejected.ClassName) {
                this.addOutput(document.getElementById("Output"), message.RejectionMessage, "error-text");
            }
            if (message.MessageName == KMud.ServerMessage.ClassName) {
                this.addOutput(document.getElementById("Output"), message.Contents);
            }
        };
        Game.prototype.addOutput = function (element, text, css) {
            if (css === void 0) { css = null; }
            var span = document.createElement("span");
            span.textContent = text;
            span.className = css;
            element.appendChild(span);
            var br = document.createElement("br");
            element.appendChild(br);
        };
        return Game;
    })();
    KMud.Game = Game;
})(KMud || (KMud = {}));
