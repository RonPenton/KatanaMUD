/// <reference path="jquery.d.ts" />
var KMud;
(function (KMud) {
    var Game = (function () {
        function Game() {
            var _this = this;
            this.lastCommands = [];
            this.currentCommand = -1;
            this.messageHandlers = {};
            this.commandHandlers = {};
            this.symbolCommandHandlers = {};
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
            this.registerMessageHandlers();
            this.registerCommandHandlers();
        }
        Game.prototype.processCommand = function (command) {
            var lower = command.toLocaleLowerCase().trim();
            var words = command.split(/\s+/gi);
            var tail = command.substr(words[0].length).trim();
            var param = words[0].substr(1);
            var handler = this.commandHandlers[words[0]];
            if (handler) {
                handler(words, tail, words.length > 1 ? words[1] : null);
                return;
            }
            // Check for character commands; ie "/Mithrandir hey there"
            var char = words[0].substr(0, 1);
            var charHandler = this.symbolCommandHandlers[char];
            if (charHandler != null) {
                charHandler(words, command.substr(1), param);
                return;
            }
            // No clue. Say it, don't spray it.
            this.talk(command, KMud.CommunicationType.Say);
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
            var handler = this.messageHandlers[message.MessageName];
            if (handler != null) {
                handler(message);
            }
        };
        Game.prototype.registerMessageHandlers = function () {
            var _this = this;
            this.messageHandlers[KMud.LoginRejected.ClassName] = function (message) {
                _this.addOutput(document.getElementById("Output"), message.RejectionMessage, "error-text");
                if (message.NoCharacter == true) {
                    window.location.replace("/Home/ChooseRace");
                }
            };
            this.messageHandlers[KMud.ServerMessage.ClassName] = function (message) {
                _this.addOutput(document.getElementById("Output"), message.Contents);
            };
            this.messageHandlers[KMud.PongMessage.ClassName] = function (message) {
                var latency = new Date().valueOf() - new Date(message.SendTime).valueOf();
                _this.addOutput(document.getElementById("Output"), "Ping: Latency " + latency + "ms", "system-text");
            };
            this.messageHandlers[KMud.RoomDescriptionMessage.ClassName] = function (message) { return _this.showRoomDescription(message); };
            this.messageHandlers[KMud.CommunicationMessage.ClassName] = function (message) { return _this.showCommunication(message); };
            this.messageHandlers[KMud.ActorInformationMessage.ClassName] = function (message) { return _this.currentPlayer = message; };
        };
        Game.prototype.registerCommandHandlers = function () {
            var _this = this;
            this.commandHandlers["ping"] = function (x) { return _this.ping(); };
            this.commandHandlers["n"] = this.commandHandlers["north"] = function (words) { return _this.move(KMud.Direction.North); };
            this.commandHandlers["s"] = this.commandHandlers["south"] = function (words) { return _this.move(KMud.Direction.South); };
            this.commandHandlers["e"] = this.commandHandlers["east"] = function (words) { return _this.move(KMud.Direction.East); };
            this.commandHandlers["w"] = this.commandHandlers["west"] = function (words) { return _this.move(KMud.Direction.West); };
            this.commandHandlers["ne"] = this.commandHandlers["northeast"] = function (words) { return _this.move(KMud.Direction.Northeast); };
            this.commandHandlers["nw"] = this.commandHandlers["northwest"] = function (words) { return _this.move(KMud.Direction.Northwest); };
            this.commandHandlers["se"] = this.commandHandlers["southeast"] = function (words) { return _this.move(KMud.Direction.Southeast); };
            this.commandHandlers["sw"] = this.commandHandlers["southwest"] = function (words) { return _this.move(KMud.Direction.Southwest); };
            this.commandHandlers["u"] = this.commandHandlers["up"] = function (words) { return _this.move(KMud.Direction.Up); };
            this.commandHandlers["d"] = this.commandHandlers["down"] = function (words) { return _this.move(KMud.Direction.Down); };
            this.commandHandlers["l"] = this.commandHandlers["look"] = function (words) { return _this.look(words); };
            this.commandHandlers["gos"] = this.commandHandlers["gossip"] = function (words, tail) { return _this.talk(tail, KMud.CommunicationType.Gossip); };
            this.commandHandlers["say"] = function (words, tail) { return _this.talk(tail, KMud.CommunicationType.Say); };
            this.symbolCommandHandlers["."] = function (words, tail) { return _this.talk(tail, KMud.CommunicationType.Say); };
            this.symbolCommandHandlers["/"] = function (words, tail, param) { return _this.talk(tail, KMud.CommunicationType.Telepath, param); };
        };
        Game.prototype.talk = function (text, type, param) {
            if (StringUtilities.isNullOrWhitespace(text))
                return; // don't bother sending empty messages.
            var message = new KMud.CommunicationMessage();
            message.Message = text;
            message.Type = type;
            if (param !== undefined) {
                message.ActorName = param;
            }
            this._socket.send(JSON.stringify(message));
        };
        Game.prototype.look = function (words) {
            var message = new KMud.LookMessage();
            //TODO: Parse parameters
            this._socket.send(JSON.stringify(message));
        };
        Game.prototype.move = function (direction) {
            var message = new KMud.MoveMessage();
            message.Direction = direction;
            this._socket.send(JSON.stringify(message));
        };
        Game.prototype.ping = function () {
            var message = new KMud.PingMessage();
            message.SendTime = new Date();
            this._socket.send(JSON.stringify(message));
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
            var _this = this;
            if (message.CannotSee) {
                this.mainOutput(message.CannotSeeMessage, "cannot-see");
            }
            else {
                this.mainOutput(message.Name, "room-name");
                if (StringUtilities.notEmpty(message.Description)) {
                    this.mainOutput(message.Description, "room-desc");
                }
                if (message.Actors.length > 1) {
                    var actors = message.Actors.filter(function (x) { return x.Id != _this.currentPlayer.Id; });
                    this.mainOutput("Also here: " + actors.map(function (x) { return x.Name; }).join(", ") + ".", "actors");
                }
                if (message.Exits.length > 0) {
                    this.mainOutput("Obvious exits: " + message.Exits.map(function (x) { return x.Name; }).join(", "), "exits");
                }
                else {
                    this.mainOutput("Obvious exits: NONE!!!", "exits");
                }
            }
        };
        Game.prototype.showCommunication = function (message) {
            switch (message.Type) {
                case KMud.CommunicationType.Gossip:
                    this.mainOutput(message.ActorName + " gossips: " + message.Message, "gossip");
                    break;
                case KMud.CommunicationType.Say:
                    if (message.ActorId == this.currentPlayer.Id) {
                        this.mainOutput("You say \"" + message.Message + "\"", "say");
                    }
                    else {
                        this.mainOutput(message.ActorName + " says \"" + message.Message + "\"", "say");
                    }
                    break;
                case KMud.CommunicationType.Telepath:
                    this.mainOutput(message.ActorName + " telepaths " + message.Message, "telepath");
                    break;
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
