/// <reference path="jquery.d.ts" />
var KMud;
(function (KMud) {
    var Game = (function () {
        function Game() {
            var _this = this;
            this.lastCommands = [];
            this.currentCommand = -1;
            this.commandHandlers = {};
            this.input = document.getElementById("InputBox");
            $("#InputBox").keypress(function (x) {
                if (x.keyCode == 13 && _this.input.value.trim() != "") {
                    _this.addOutput(document.getElementById("Output"), _this.input.value, "command-text");
                    _this.lastCommands.unshift(_this.input.value);
                    _this.processCommand(_this.input.value);
                    _this.input.value = "";
                    _this.currentCommand = -1;
                    while (_this.lastCommands.length > 20) {
                        _this.lastCommands.shift();
                    }
                }
            }).keydown(function (x) {
                if (x.keyCode == 38 && x.ctrlKey) {
                    if (_this.currentCommand < _this.lastCommands.length - 1) {
                        _this.currentCommand++;
                        _this.input.value = _this.lastCommands[_this.currentCommand];
                    }
                }
                else if (x.keyCode == 40 && x.ctrlKey) {
                    if (_this.currentCommand >= 1) {
                        _this.currentCommand--;
                        _this.input.value = _this.lastCommands[_this.currentCommand];
                    }
                    else if (_this.currentCommand == 0) {
                        _this.input.value = "";
                        _this.currentCommand = -1;
                    }
                }
            });
            $(window).mouseup(function (x) {
                if (window.getSelection().toString().length == 0)
                    $("#InputBox").focus();
            });
            this.registerHandlers();
        }
        Game.prototype.processCommand = function (command) {
            var lower = command.toLocaleLowerCase().trim();
            var words = command.split(/\s+/gi);
            if (words[0] == "ping") {
                var message = new KMud.PingMessage();
                message.SendTime = new Date();
                this._socket.send(JSON.stringify(message));
            }
        };
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
            var handler = this.commandHandlers[message.MessageName];
            if (handler != null) {
                handler(message);
            }
        };
        Game.prototype.registerHandlers = function () {
            var _this = this;
            this.commandHandlers[KMud.LoginRejected.ClassName] = function (message) {
                _this.addOutput(document.getElementById("Output"), message.RejectionMessage, "error-text");
                if (message.NoCharacter == true) {
                    window.location.replace("/Home/ChooseRace");
                }
            };
            this.commandHandlers[KMud.ServerMessage.ClassName] = function (message) {
                _this.addOutput(document.getElementById("Output"), message.Contents);
            };
            this.commandHandlers[KMud.PongMessage.ClassName] = function (message) {
                var latency = new Date().valueOf() - new Date(message.SendTime).valueOf();
                _this.addOutput(document.getElementById("Output"), "Ping: Latency " + latency + "ms", "system-text");
            };
            this.commandHandlers[KMud.RoomDescriptionMessage.ClassName] = function (message) { return _this.showRoomDescription(message); };
        };
        Game.prototype.addOutput = function (element, text, css) {
            if (css === void 0) { css = null; }
            var scrolledToBottom = (element.scrollHeight - element.scrollTop === element.offsetHeight);
            var span = document.createElement("span");
            span.textContent = text;
            span.className = css;
            element.appendChild(span);
            var br = document.createElement("br");
            element.appendChild(br);
            // Keep scrolling to bottom if they're already there.
            if (scrolledToBottom) {
                element.scrollTop = element.scrollHeight;
            }
        };
        Game.prototype.mainOutput = function (text, css) {
            if (css === void 0) { css = null; }
            this.addOutput(document.getElementById("Output"), text, css);
        };
        Game.prototype.showRoomDescription = function (message) {
            if (message.CannotSee) {
                this.mainOutput(message.CannotSeeMessage, "cannot-see");
            }
            else {
                this.mainOutput(message.Name, "room-name");
                if (StringUtilities.notEmpty(message.Description)) {
                    this.mainOutput(message.Description, "room-desc");
                }
            }
        };
        return Game;
    })();
    KMud.Game = Game;
    var StringUtilities = (function () {
        function StringUtilities() {
        }
        StringUtilities.notEmpty = function (s) {
            return !this.isNullOrWhitespace(s);
        };
        StringUtilities.isNullOrEmpty = function (s) {
            return (s === null || s === undefined || s === "");
        };
        StringUtilities.isNullOrWhitespace = function (s) {
            return (s === null || s === undefined || /^\s*$/g.test(s));
        };
        StringUtilities.formatString = function (str, args) {
            return str.replace(/{(\d+)}/g, function (match) {
                var i = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    i[_i - 1] = arguments[_i];
                }
                return typeof args[i[0]] != 'undefined' ? args[i[0]] : match;
            });
        };
        StringUtilities.escapeRegExp = function (str) {
            return str.replace(StringUtilities._escapeRegExpRegexp, "\\$&");
        };
        // Referring to the table here:
        // https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/regexp
        // these characters should be escaped
        // \ ^ $ * + ? . ( ) | { } [ ]
        // These characters only have special meaning inside of brackets
        // they do not need to be escaped, but they MAY be escaped
        // without any adverse effects (to the best of my knowledge and casual testing)
        // : ! , = 
        StringUtilities._escapeRegExpCharacters = [
            "-",
            "[",
            "]",
            "/",
            "{",
            "}",
            "(",
            ")",
            "*",
            "+",
            "?",
            ".",
            "\\",
            "^",
            "$",
            "|"
        ];
        StringUtilities._escapeRegExpRegexp = new RegExp('[' + StringUtilities._escapeRegExpCharacters.join('\\') + ']', 'g');
        return StringUtilities;
    })();
    KMud.StringUtilities = StringUtilities;
})(KMud || (KMud = {}));
