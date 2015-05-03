/// <reference path="jquery.d.ts" />
/// <reference path="utilities/linq.ts" />
//TODO: At some point this file will need to be broken up. Right now ASP.NET 5 doesn't have the right tooling to merge typescript files, so it's a massive pain 
// in the ass. When (if?) the tooling is added, separate the file into logical blocks. 
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
                if (x.keyCode == 13) {
                    if (_this.input.value.trim() != "") {
                        _this.addOutput(document.getElementById("Output"), _this.input.value, "command-text");
                        _this.lastCommands.unshift(_this.input.value);
                        _this.processCommand(_this.input.value);
                        _this.input.value = "";
                        _this.currentCommand = -1;
                        while (_this.lastCommands.length > 20) {
                            _this.lastCommands.shift();
                        }
                    }
                    else {
                        // Hardcoded "brief look" command when user simply hits "enter".
                        _this.look([], true);
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
            this.talk(command, 2 /* Say */);
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
            this.messageHandlers[KMud.PongMessage.ClassName] = function (message) {
                var latency = new Date().valueOf() - new Date(message.SendTime).valueOf();
                _this.addOutput(document.getElementById("Output"), "Ping: Latency " + latency + "ms", "system-text");
            };
            this.messageHandlers[KMud.ServerMessage.ClassName] = function (message) { return _this.addOutput(document.getElementById("Output"), message.Contents); };
            this.messageHandlers[KMud.RoomDescriptionMessage.ClassName] = function (message) { return _this.showRoomDescription(message); };
            this.messageHandlers[KMud.CommunicationMessage.ClassName] = function (message) { return _this.showCommunication(message); };
            this.messageHandlers[KMud.ActorInformationMessage.ClassName] = function (message) { return _this.currentPlayer = message; };
            this.messageHandlers[KMud.InventoryListMessage.ClassName] = function (message) { return _this.showInventory(message); };
            this.messageHandlers[KMud.LoginStateMessage.ClassName] = function (message) { return _this.loginMessage(message); };
            this.messageHandlers[KMud.PartyMovementMessage.ClassName] = function (message) { return _this.partyMovement(message); };
            this.messageHandlers[KMud.ItemOwnershipMessage.ClassName] = function (message) { return _this.itemOwnership(message); };
            this.messageHandlers[KMud.CashTransferMessage.ClassName] = function (message) { return _this.cashTransfer(message); };
            this.messageHandlers[KMud.ActionNotAllowedMessage.ClassName] = function (message) { return _this.mainOutput(message.Message, "action-not-allowed"); };
            this.messageHandlers[KMud.GenericMessage.ClassName] = function (message) { return _this.mainOutput(message.Message, message.Class); };
        };
        Game.prototype.registerCommandHandlers = function () {
            var _this = this;
            this.commandHandlers["ping"] = function (x) { return _this.ping(); };
            this.commandHandlers["n"] = this.commandHandlers["north"] = function (words) { return _this.move(0 /* North */); };
            this.commandHandlers["s"] = this.commandHandlers["south"] = function (words) { return _this.move(1 /* South */); };
            this.commandHandlers["e"] = this.commandHandlers["east"] = function (words) { return _this.move(2 /* East */); };
            this.commandHandlers["w"] = this.commandHandlers["west"] = function (words) { return _this.move(3 /* West */); };
            this.commandHandlers["ne"] = this.commandHandlers["northeast"] = function (words) { return _this.move(4 /* Northeast */); };
            this.commandHandlers["nw"] = this.commandHandlers["northwest"] = function (words) { return _this.move(5 /* Northwest */); };
            this.commandHandlers["se"] = this.commandHandlers["southeast"] = function (words) { return _this.move(6 /* Southeast */); };
            this.commandHandlers["sw"] = this.commandHandlers["southwest"] = function (words) { return _this.move(7 /* Southwest */); };
            this.commandHandlers["u"] = this.commandHandlers["up"] = function (words) { return _this.move(8 /* Up */); };
            this.commandHandlers["d"] = this.commandHandlers["down"] = function (words) { return _this.move(9 /* Down */); };
            this.commandHandlers["l"] = this.commandHandlers["look"] = function (words) { return _this.look(words); };
            this.commandHandlers["i"] = this.commandHandlers["inv"] = this.commandHandlers["inventory"] = function (words, tail) { return _this.SendMessage(new KMud.InventoryCommand()); };
            this.commandHandlers["get"] = this.commandHandlers["g"] = function (words, tail) { return _this.get(tail); };
            this.commandHandlers["drop"] = this.commandHandlers["dr"] = function (words, tail) { return _this.drop(tail, false); };
            this.commandHandlers["hide"] = this.commandHandlers["hid"] = function (words, tail) { return _this.drop(tail, true); };
            this.commandHandlers["gos"] = this.commandHandlers["gossip"] = function (words, tail) { return _this.talk(tail, 0 /* Gossip */); };
            this.commandHandlers["say"] = function (words, tail) { return _this.talk(tail, 2 /* Say */); };
            this.commandHandlers["sys"] = this.commandHandlers["sysop"] = function (words, tail) {
                var msg = new KMud.SysopMessage();
                msg.Command = tail;
                _this.SendMessage(msg);
            };
            this.symbolCommandHandlers["."] = function (words, tail) { return _this.talk(tail, 2 /* Say */); };
            this.symbolCommandHandlers["/"] = function (words, tail, param) { return _this.talk(tail, 8 /* Telepath */, param); };
        };
        Game.prototype.loginMessage = function (message) {
            if (message.Login == true) {
                this.mainOutput(message.Actor.Name + " just entered the Realm.", "login");
            }
            else {
                this.mainOutput(message.Actor.Name + " just left the Realm.", "logout");
            }
        };
        Game.prototype.get = function (item) {
            var message = new KMud.GetItemCommand();
            var tokens = item.split(/\s+/gi);
            var quantity = parseInt(tokens[0]);
            if (!isNaN(quantity)) {
                message.Quantity = quantity;
                message.ItemName = tokens.slice(1).join(" ");
            }
            else {
                message.ItemName = item;
            }
            this.SendMessage(message);
        };
        Game.prototype.drop = function (item, hide) {
            var message = new KMud.DropItemCommand();
            message.Hide = hide;
            var tokens = item.split(/\s+/gi);
            var quantity = parseInt(tokens[0]);
            if (!isNaN(quantity)) {
                message.Quantity = quantity;
                message.ItemName = tokens.slice(1).join(" ");
            }
            else {
                message.ItemName = item;
            }
            this.SendMessage(message);
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
        Game.prototype.look = function (words, brief) {
            if (brief === void 0) { brief = false; }
            var message = new KMud.LookMessage();
            message.Brief = brief;
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
        Game.prototype.partyMovement = function (message) {
            var _this = this;
            var actors = KMud.Linq(message.Actors);
            if (actors.areAny(function (x) { return x.Id == _this.currentPlayer.Id; })) {
                // You're the one moving.
                // Don't show anything if you move. I guess. That's how MajorMUD works. 
                if (message.Enter == false && message.Leader.Id != this.currentPlayer.Id) {
                    // else show the party lead message.
                    this.mainOutput(" -- Following your Party leader " + KMud.Direction[message.Direction].toLowerCase() + " --", "party-follow");
                }
            }
            else {
                var runs = this.runJoin(actors.select(function (x) { return x.Name; }).toArray(), ", ", "moving-player", "player-separator");
                if (message.Enter) {
                    var verb = "walks";
                    if (message.Actors.length > 1)
                        verb = "walk";
                    runs.push([" " + verb + " into the room from the " + KMud.Direction[message.Direction].toLowerCase() + ".", "party-movement"]);
                }
                else {
                    runs.push([" just left to the " + KMud.Direction[message.Direction].toLowerCase() + ".", "party-movement"]);
                }
                this.mainOutputRuns(runs);
            }
        };
        Game.prototype.itemOwnership = function (message) {
            var groups = KMud.Linq(message.Items).groupBy(function (x) { return x.TemplateId + "|" + x.Name; }).toArray();
            for (var i = 0; i < groups.length; i++) {
                var name = groups[i].values[0].Name;
                if (groups[i].values.length > 1) {
                    name = String(groups[i].values.length) + " " + name;
                }
                if (message.Giver != null && message.Taker != null) {
                    // player-to-player transfer
                    if (message.Giver.Id == this.currentPlayer.Id) {
                        this.mainOutput("You just gave " + name + " to " + message.Taker.Name + ".", "item-ownership");
                    }
                    else if (message.Taker.Id == this.currentPlayer.Id) {
                        this.mainOutput(message.Giver.Name + " just gave you " + name + ".", "item-ownership");
                    }
                    else {
                        this.mainOutput(message.Giver.Name + " just gave " + message.Taker.Name + " something.", "item-ownership");
                    }
                }
                else {
                    if (message.Giver != null && message.Giver.Id == this.currentPlayer.Id) {
                        var verb = "dropped";
                        if (message.Hide)
                            verb = "hid";
                        this.mainOutput("You " + verb + " " + name + ".", "item-ownership");
                    }
                    else if (message.Taker != null && message.Taker.Id == this.currentPlayer.Id) {
                        this.mainOutput("You took " + name + ".", "item-ownership");
                    }
                    else if (message.Giver != null) {
                        this.mainOutput(message.Giver.Name + " dropped " + name + ".", "item-ownership");
                    }
                    else if (message.Taker != null) {
                        this.mainOutput(message.Taker.Name + " picks up " + name + ".", "item-ownership");
                    }
                }
            }
        };
        Game.prototype.cashTransfer = function (message) {
            var name = message.Currency.Name;
            if (message.Quantity > 0) {
                name = String(message.Quantity) + " " + name;
                if (message.Quantity > 1) {
                    name = name + "s";
                }
            }
            else {
                name = "some " + name + "s";
            }
            if (message.Giver != null && message.Taker != null) {
                // player-to-player transfer
                if (message.Giver.Id == this.currentPlayer.Id) {
                    this.mainOutput("You just gave " + name + " to " + message.Taker.Name + ".", "item-ownership");
                }
                else if (message.Taker.Id == this.currentPlayer.Id) {
                    this.mainOutput(message.Giver.Name + " just gave you " + name + ".", "item-ownership");
                }
                else {
                    this.mainOutput(message.Giver.Name + " just gave " + message.Taker.Name + " something.", "item-ownership");
                }
            }
            else {
                if (message.Giver != null && message.Giver.Id == this.currentPlayer.Id) {
                    var verb = "dropped";
                    if (message.Hide)
                        verb = "hid";
                    this.mainOutput("You " + verb + " " + name + ".", "item-ownership");
                }
                else if (message.Taker != null && message.Taker.Id == this.currentPlayer.Id) {
                    this.mainOutput("You took " + name + ".", "item-ownership");
                }
                else if (message.Giver != null) {
                    this.mainOutput(message.Giver.Name + " dropped " + name + ".", "item-ownership");
                }
                else if (message.Taker != null) {
                    this.mainOutput(message.Taker.Name + " picks up " + name + ".", "item-ownership");
                }
            }
        };
        Game.prototype.addElements = function (target, elements) {
            var scrolledToBottom = (target.scrollHeight - target.scrollTop === target.offsetHeight);
            for (var i = 0; i < elements.length; i++) {
                target.appendChild(elements[i]);
            }
            // Keep scrolling to bottom if they're already there.
            if (scrolledToBottom) {
                target.scrollTop = target.scrollHeight;
            }
        };
        Game.prototype.addOutput = function (target, text, css) {
            if (css === void 0) { css = null; }
            this.addOutputRuns(target, [[text, css]]);
        };
        Game.prototype.addOutputRuns = function (target, runs) {
            var elements = [];
            for (var i = 0; i < runs.length; i++) {
                var span = document.createElement("span");
                span.textContent = runs[i][0];
                if (runs[i].length > 1)
                    span.className = runs[i][1];
                elements.push(span);
            }
            elements.push(document.createElement("br"));
            this.addElements(target, elements);
        };
        Game.prototype.runJoin = function (items, separator, itemClass, separatorClass) {
            var output = [];
            for (var i = 0; i < items.length - 1; i++) {
                output.push([items[i], itemClass]);
                output.push([separator, separatorClass]);
            }
            output.push([items[items.length - 1], itemClass]);
            return output;
        };
        Game.prototype.mainOutput = function (text, css) {
            if (css === void 0) { css = null; }
            this.addOutput(document.getElementById("Output"), text, css);
        };
        Game.prototype.mainOutputRuns = function (runs) {
            this.addOutputRuns(document.getElementById("Output"), runs);
        };
        Game.prototype.showRoomDescription = function (message) {
            var _this = this;
            if (message.CannotSee) {
                if (!StringUtilities.isNullOrWhitespace(message.CannotSeeMessage)) {
                    this.mainOutput(message.CannotSeeMessage, "cannot-see");
                }
                else {
                    if (message.LightLevel == -10000 /* Nothing */)
                        this.mainOutput("The room is darker than anything you've ever seen before - you can't see anything", "cannot-see");
                    if (message.LightLevel == -500 /* PitchBlack */)
                        this.mainOutput("The room is pitch black - you can't see anything", "cannot-see");
                    if (message.LightLevel == -250 /* VeryDark */)
                        this.mainOutput("The room is very dark - you can't see anything", "cannot-see");
                }
            }
            else {
                this.mainOutput(message.Name, "room-name");
                if (StringUtilities.notEmpty(message.Description)) {
                    this.mainOutput(message.Description, "room-desc");
                }
                var items = "";
                if (message.VisibleCash.length > 0) {
                    items = KMud.Linq(message.VisibleCash).select(function (x) { return String(x.Amount) + " " + x.Name + (x.Amount > 1 ? "s" : ""); }).toArray().join(", ");
                }
                if (message.VisibleItems.length > 0) {
                    if (items.length > 0)
                        items += ", ";
                    var groups = KMud.Linq(message.VisibleItems).groupBy(function (x) { return x.TemplateId + "|" + x.Name; });
                    items += groups.select(function (x) { return (x.values.length > 1 ? String(x.values.length) + " " : "") + x.values[0].Name; }).toArray().join(", ");
                }
                if (items.length > 0) {
                    this.mainOutput("You notice " + items + " here.", "items");
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
                if (message.LightLevel == -200 /* BarelyVisible */)
                    this.mainOutput("The room is barely visible", "dimly-lit");
                if (message.LightLevel == -150 /* DimlyLit */)
                    this.mainOutput("The room is dimly lit", "dimly-lit");
            }
        };
        Game.prototype.showCommunication = function (message) {
            switch (message.Type) {
                case 0 /* Gossip */:
                    this.mainOutput(message.ActorName + " gossips: " + message.Message, "gossip");
                    break;
                case 2 /* Say */:
                    if (message.ActorId == this.currentPlayer.Id) {
                        this.mainOutput("You say \"" + message.Message + "\"", "say");
                    }
                    else {
                        this.mainOutput(message.ActorName + " says \"" + message.Message + "\"", "say");
                    }
                    break;
                case 8 /* Telepath */:
                    this.mainOutput(message.ActorName + " telepaths " + message.Message, "telepath");
                    break;
            }
        };
        Game.prototype.showInventory = function (message) {
            var groups = KMud.Linq(message.Items).groupBy(function (x) { return x.TemplateId + "|" + x.Name; });
            var items = "";
            if (message.Cash.length > 0) {
                items = KMud.Linq(message.Cash).select(function (x) { return String(x.Amount) + " " + x.Name + (x.Amount > 1 ? "s" : ""); }).toArray().join(", ");
            }
            if (message.Items.length > 0) {
                if (items.length > 0)
                    items += ", ";
                items += groups.select(function (x) { return (x.values.length > 1 ? String(x.values.length) + " " : "") + x.values[0].Name; }).toArray().join(", ");
            }
            var str = "You are carrying " + items;
            this.mainOutput(str, "inventory");
            //TODO: You have no keys.
            //TODO: Wealth: 500 copper farthings
            var pct = Math.round((message.Encumbrance / message.MaxEncumbrance) * 100);
            var category = "Heavy";
            if (pct <= 66)
                category = "Medium";
            else if (pct <= 33)
                category = "Light";
            else if (pct <= 15)
                category = "None";
            var str = "Encumbrance: " + message.Encumbrance + " / " + message.MaxEncumbrance + " - " + category + "[" + pct + "%]";
            this.mainOutput(str, "inventory");
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
