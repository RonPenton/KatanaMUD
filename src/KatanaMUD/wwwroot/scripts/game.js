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
            this.ding = new Audio("../media/ding.wav");
        }
        Object.defineProperty(Game.prototype, "mainOutput", {
            get: function () {
                return document.getElementById("Output");
            },
            enumerable: true,
            configurable: true
        });
        Game.prototype.notify = function () {
            //if (document.visibilityState == "hidden") {
            this.ding.play();
            //}
        };
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
            this.messageHandlers[KMud.SearchMessage.ClassName] = function (message) { return _this.search(message); };
            this.messageHandlers[KMud.ActionNotAllowedMessage.ClassName] = function (message) { return _this.addOutput(_this.mainOutput, message.Message, "action-not-allowed"); };
            this.messageHandlers[KMud.GenericMessage.ClassName] = function (message) { return _this.addOutput(_this.mainOutput, message.Message, message.Class); };
            this.messageHandlers[KMud.AmbiguousActorMessage.ClassName] = function (message) { return _this.ambiguousActors(message); };
            this.messageHandlers[KMud.AmbiguousItemMessage.ClassName] = function (message) { return _this.ambiguousItems(message); };
            this.messageHandlers[KMud.ItemEquippedChangedMessage.ClassName] = function (message) { return _this.equipChanged(message); };
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
            this.commandHandlers["i"] = this.commandHandlers["inv"] = this.commandHandlers["inventory"] = function (words, tail) { return _this.SendMessage(new KMud.InventoryCommand()); };
            this.commandHandlers["get"] = this.commandHandlers["g"] = function (words, tail) { return _this.get(tail); };
            this.commandHandlers["drop"] = this.commandHandlers["dr"] = function (words, tail) { return _this.drop(tail, false); };
            this.commandHandlers["hide"] = this.commandHandlers["hid"] = function (words, tail) { return _this.drop(tail, true); };
            this.commandHandlers["search"] = this.commandHandlers["sea"] = function (words, tail) { return _this.SendMessage(new KMud.SearchCommand()); };
            this.commandHandlers["equip"] = this.commandHandlers["eq"] = this.commandHandlers["arm"] = this.commandHandlers["ar"] = function (words, tail) { return _this.SendMessage(new KMud.EquipCommand(null, tail)); };
            this.commandHandlers["remove"] = this.commandHandlers["rem"] = function (words, tail) { return _this.SendMessage(new KMud.RemoveCommand(null, tail)); };
            this.commandHandlers["gos"] = this.commandHandlers["gossip"] = function (words, tail) { return _this.talk(tail, KMud.CommunicationType.Gossip); };
            this.commandHandlers["say"] = function (words, tail) { return _this.talk(tail, KMud.CommunicationType.Say); };
            this.commandHandlers["sys"] = this.commandHandlers["sysop"] = function (words, tail) {
                var msg = new KMud.SysopMessage();
                msg.Command = tail;
                _this.SendMessage(msg);
            };
            this.symbolCommandHandlers["."] = function (words, tail) { return _this.talk(tail, KMud.CommunicationType.Say); };
            this.symbolCommandHandlers["/"] = function (words, tail, param) { return _this.talk(tail, KMud.CommunicationType.Telepath, param); };
        };
        Game.prototype.loginMessage = function (message) {
            if (message.Login == true) {
                this.addOutput(this.mainOutput, message.Actor.Name + " just entered the Realm.", "login");
            }
            else {
                this.addOutput(this.mainOutput, message.Actor.Name + " just left the Realm.", "logout");
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
                    this.addOutput(this.mainOutput, " -- Following your Party leader " + KMud.Direction[message.Direction].toLowerCase() + " --", "party-follow");
                }
            }
            else {
                // TODO: Player Link
                var runs = this.createElements(actors.toArray(), function (x) { return x.Name; }, "moving-player", "a");
                var runs = this.joinElements(runs, ", ", "player-separator");
                if (message.Enter) {
                    var verb = "walks";
                    if (message.Actors.length > 1)
                        verb = "walk";
                    runs.push(this.createElement(" " + verb + " into the room from the " + KMud.Direction[message.Direction].toLowerCase() + ".", "party-movement"));
                }
                else {
                    runs.push(this.createElement(" just left to the " + KMud.Direction[message.Direction].toLowerCase() + ".", "party-movement"));
                }
                this.addElements(this.mainOutput, runs);
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
                        this.addOutput(this.mainOutput, "You just gave " + name + " to " + message.Taker.Name + ".", "item-ownership");
                    }
                    else if (message.Taker.Id == this.currentPlayer.Id) {
                        this.addOutput(this.mainOutput, message.Giver.Name + " just gave you " + name + ".", "item-ownership");
                    }
                    else {
                        this.addOutput(this.mainOutput, message.Giver.Name + " just gave " + message.Taker.Name + " something.", "item-ownership");
                    }
                }
                else {
                    if (message.Giver != null && message.Giver.Id == this.currentPlayer.Id) {
                        var verb = "dropped";
                        if (message.Hide)
                            verb = "hid";
                        this.addOutput(this.mainOutput, "You " + verb + " " + name + ".", "item-ownership");
                    }
                    else if (message.Taker != null && message.Taker.Id == this.currentPlayer.Id) {
                        this.addOutput(this.mainOutput, "You took " + name + ".", "item-ownership");
                    }
                    else if (message.Giver != null) {
                        this.addOutput(this.mainOutput, message.Giver.Name + " dropped " + name + ".", "item-ownership");
                    }
                    else if (message.Taker != null) {
                        this.addOutput(this.mainOutput, message.Taker.Name + " picks up " + name + ".", "item-ownership");
                    }
                }
            }
        };
        Game.prototype.cashTransfer = function (message) {
            var name = message.Currency.Name;
            if (message.Quantity > 0) {
                name = this.pluralize(name, message.Quantity);
            }
            else {
                name = "some " + this.pluralize(name, 0, true);
            }
            if (message.Giver != null && message.Taker != null) {
                // player-to-player transfer
                if (message.Giver.Id == this.currentPlayer.Id) {
                    this.addOutput(this.mainOutput, "You just gave " + name + " to " + message.Taker.Name + ".", "item-ownership");
                }
                else if (message.Taker.Id == this.currentPlayer.Id) {
                    this.addOutput(this.mainOutput, message.Giver.Name + " just gave you " + name + ".", "item-ownership");
                }
                else {
                    this.addOutput(this.mainOutput, message.Giver.Name + " just gave " + message.Taker.Name + " something.", "item-ownership");
                }
            }
            else {
                if (message.Giver != null && message.Giver.Id == this.currentPlayer.Id) {
                    var verb = "dropped";
                    if (message.Hide)
                        verb = "hid";
                    this.addOutput(this.mainOutput, "You " + verb + " " + name + ".", "item-ownership");
                }
                else if (message.Taker != null && message.Taker.Id == this.currentPlayer.Id) {
                    this.addOutput(this.mainOutput, "You took " + name + ".", "item-ownership");
                }
                else if (message.Giver != null) {
                    this.addOutput(this.mainOutput, message.Giver.Name + " dropped " + name + ".", "item-ownership");
                }
                else if (message.Taker != null) {
                    this.addOutput(this.mainOutput, message.Taker.Name + " picks up " + name + ".", "item-ownership");
                }
            }
        };
        Game.prototype.search = function (message) {
            if (message.FoundCash.length == 0 && message.FoundItems.length == 0) {
                this.addOutput(this.mainOutput, "Your search revealed nothing.", "search");
                return;
            }
            var items = [];
            for (var i = 0; i < message.FoundCash.length; i++) {
                items.push(message.FoundCash[i].Amount + " " + message.FoundCash[i].Name);
            }
            if (message.FoundItems.length > 0) {
                var groups = KMud.Linq(message.FoundItems).groupBy(function (x) { return x.TemplateId + "|" + x.Name; });
                KMud.pushRange(items, groups.select(function (x) { return (x.values.length > 1 ? String(x.values.length) + " " : "") + x.values[0].Name; }).toArray());
            }
            this.addOutput(this.mainOutput, "You notice " + items.join(", ") + " here.", "search-found");
        };
        Game.prototype.showRoomDescription = function (message) {
            var _this = this;
            if (message.CannotSee) {
                if (!StringUtilities.isNullOrWhitespace(message.CannotSeeMessage)) {
                    this.addOutput(this.mainOutput, message.CannotSeeMessage, "cannot-see");
                }
                else {
                    if (message.LightLevel == KMud.LightLevel.Nothing)
                        this.addOutput(this.mainOutput, "The room is darker than anything you've ever seen before - you can't see anything", "cannot-see");
                    if (message.LightLevel == KMud.LightLevel.PitchBlack)
                        this.addOutput(this.mainOutput, "The room is pitch black - you can't see anything", "cannot-see");
                    if (message.LightLevel == KMud.LightLevel.VeryDark)
                        this.addOutput(this.mainOutput, "The room is very dark - you can't see anything", "cannot-see");
                }
            }
            else {
                this.addOutput(this.mainOutput, message.Name, "room-name");
                if (StringUtilities.notEmpty(message.Description)) {
                    this.addOutput(this.mainOutput, message.Description, "room-desc");
                }
                // Items
                var visibleGroups = this.groupItems(message.VisibleItems);
                var foundGroups = this.groupItems(message.FoundItems);
                var itemRuns = [];
                KMud.pushRange(itemRuns, this.createElements(message.VisibleCash, function (x) { return _this.pluralize(x.Name, x.Amount); }, "item-cash", "a"));
                KMud.pushRange(itemRuns, this.createElements(message.FoundCash, function (x) { return _this.pluralize(x.Name, x.Amount); }, "item-cash-hidden", "a"));
                KMud.pushRange(itemRuns, this.createElements(visibleGroups.toArray(), function (x) { return _this.pluralize(x.values[0].Name, x.values.length); }, "item-visible", "a"));
                KMud.pushRange(itemRuns, this.createElements(foundGroups.toArray(), function (x) { return _this.pluralize(x.values[0].Name, x.values.length); }, "item-hidden", "a"));
                itemRuns = this.joinElements(itemRuns, ", ", "item-separator");
                if (itemRuns.length != 0) {
                    this.addOutput(this.mainOutput, "You notice ", "items", false);
                    this.addElements(this.mainOutput, itemRuns, false);
                    this.addOutput(this.mainOutput, " here.", "items");
                }
                // Actors
                if (message.Actors.length > 1) {
                    var actors = message.Actors.filter(function (x) { return x.Id != _this.currentPlayer.Id; });
                    this.addOutput(this.mainOutput, "Also here: " + actors.map(function (x) { return x.Name; }).join(", ") + ".", "actors");
                }
                // Exits
                if (message.Exits.length > 0) {
                    this.addOutput(this.mainOutput, "Obvious exits: " + message.Exits.map(function (x) { return x.Name; }).join(", "), "exits");
                }
                else {
                    this.addOutput(this.mainOutput, "Obvious exits: NONE!!!", "exits");
                }
                if (message.LightLevel == KMud.LightLevel.BarelyVisible)
                    this.addOutput(this.mainOutput, "The room is barely visible", "dimly-lit");
                if (message.LightLevel == KMud.LightLevel.DimlyLit)
                    this.addOutput(this.mainOutput, "The room is dimly lit", "dimly-lit");
            }
        };
        Game.prototype.showCommunication = function (message) {
            switch (message.Type) {
                case KMud.CommunicationType.Gossip:
                    this.addOutput(this.mainOutput, message.ActorName + " gossips: " + message.Message, "gossip");
                    this.notify();
                    break;
                case KMud.CommunicationType.Say:
                    if (message.ActorId == this.currentPlayer.Id) {
                        this.addOutput(this.mainOutput, "You say \"" + message.Message + "\"", "say");
                    }
                    else {
                        this.addOutput(this.mainOutput, message.ActorName + " says \"" + message.Message + "\"", "say");
                        this.notify();
                    }
                    break;
                case KMud.CommunicationType.Telepath:
                    this.addOutput(this.mainOutput, message.ActorName + " telepaths " + message.Message, "telepath");
                    this.notify();
                    break;
            }
        };
        Game.prototype.showInventory = function (message) {
            var _this = this;
            var equipped = KMud.Linq(message.Items).where(function (x) { return x.EquippedSlot != null; }).orderBy(function (x) { return x.EquippedSlot; });
            var groups = KMud.Linq(message.Items).where(function (x) { return x.EquippedSlot == null; }).groupBy(function (x) { return x.TemplateId + "|" + x.Name; }).orderBy(function (x) { return x.first().Name; });
            var itemRuns = [];
            KMud.pushRange(itemRuns, this.createElements(message.Cash, function (x) { return _this.pluralize(x.Name, x.Amount); }, "item-cash", "a"));
            KMud.pushRange(itemRuns, this.createElements(equipped.toArray(), function (x) { return x.Name + " (" + KMud.EquipmentSlot[x.EquippedSlot] + ")"; }, "item-equipped", "a"));
            KMud.pushRange(itemRuns, this.createElements(groups.toArray(), function (x) { return _this.pluralize(x.values[0].Name, x.values.length); }, "item-held", "a"));
            itemRuns = this.joinElements(itemRuns, ", ", "item-separator");
            if (itemRuns.length == 0) {
                itemRuns.push(this.createElement("Nothing!", "item-none"));
            }
            itemRuns.unshift(this.createElement("You are carrying:", "inventory-label"));
            this.addElements(this.mainOutput, itemRuns);
            //TODO: You have no keys.
            this.addOutput(this.mainOutput, "Wealth:", "wealth-label", false);
            this.addOutput(this.mainOutput, this.pluralize(message.TotalCash.Name, message.TotalCash.Amount), "wealth-amount");
            var pct = Math.round((message.Encumbrance / message.MaxEncumbrance) * 100);
            var category = "Heavy";
            if (pct <= 66)
                category = "Medium";
            else if (pct <= 33)
                category = "Light";
            else if (pct <= 15)
                category = "None";
            this.addOutput(this.mainOutput, "Encumbrance:", "encumbrance-label", false);
            this.addOutput(this.mainOutput, message.Encumbrance + "/" + message.MaxEncumbrance + " - " + category + " [" + pct + "%]", "encumbrance-value");
        };
        Game.prototype.ambiguousActors = function (message) {
            this.addOutput(this.mainOutput, "Please be more specific.  You could have meant any of these:", "error");
            for (var i = 0; i < message.Actors.length; i++) {
                this.addOutput(this.mainOutput, "-- " + message.Actors[i].Name);
            }
        };
        Game.prototype.ambiguousItems = function (message) {
            this.addOutput(this.mainOutput, "Please be more specific.  You could have meant any of these:", "error");
            for (var i = 0; i < message.Items.length; i++) {
                this.addOutput(this.mainOutput, "-- " + message.Items[i].Name);
            }
        };
        Game.prototype.equipChanged = function (message) {
            if (message.Equipped) {
                if (message.Actor.Id == this.currentPlayer.Id) {
                    if (message.Item.EquippedSlot == KMud.EquipmentSlot.Weapon || message.Item.EquippedSlot == KMud.EquipmentSlot.Offhand || message.Item.EquippedSlot == KMud.EquipmentSlot.Light) {
                        this.addOutput(this.mainOutput, "You are now holding " + message.Item.Name + ".", "equip");
                    }
                    else {
                        this.addOutput(this.mainOutput, "You are now wearing " + message.Item.Name + ".", "equip");
                    }
                }
                else {
                    if (message.Item.EquippedSlot == KMud.EquipmentSlot.Weapon || message.Item.EquippedSlot == KMud.EquipmentSlot.Offhand || message.Item.EquippedSlot == KMud.EquipmentSlot.Light) {
                        this.addOutput(this.mainOutput, message.Actor.Name + " readies " + message.Item.Name + "!", "equip");
                    }
                    else {
                        this.addOutput(this.mainOutput, message.Actor.Name + " wears " + message.Item.Name + "!", "equip");
                    }
                }
            }
            else {
                if (message.Actor.Id == this.currentPlayer.Id) {
                    this.addOutput(this.mainOutput, "You have removed " + message.Item.Name + ".", "equip");
                }
                else {
                    this.addOutput(this.mainOutput, message.Actor.Name + " removes " + message.Item.Name + "!", "equip");
                }
            }
        };
        //////////////////////////////////////////////////////////////////////////////////
        // OUTPUT ROUTINES 
        /////////////////////////////////////////////////////////////////////////////////
        Game.prototype.addElements = function (target, elements, finished) {
            if (finished === void 0) { finished = true; }
            var scrolledToBottom = Math.abs((target.scrollHeight - target.scrollTop) - target.offsetHeight) < 10;
            for (var i = 0; i < elements.length; i++) {
                target.appendChild(elements[i]);
            }
            if (finished) {
                var newLine = document.createElement("br");
                target.appendChild(newLine);
            }
            // Keep scrolling to bottom if they're already there.
            if (scrolledToBottom) {
                target.scrollTop = target.scrollHeight;
            }
        };
        Game.prototype.addOutput = function (target, text, css, finished) {
            if (finished === void 0) { finished = true; }
            var element = document.createElement("span");
            element.textContent = text;
            if (css !== undefined) {
                element.className = css;
            }
            this.addElements(target, [element], finished);
        };
        /**
         * Creates an array of HTML Elements based on the provided list of items and options.
         */
        Game.prototype.createElements = function (items, caption, css, elementName, decorator) {
            if (elementName === void 0) { elementName = "span"; }
            var elements = [];
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var c;
                if (css !== undefined) {
                    if (typeof css === 'string') {
                        c = css;
                    }
                    else {
                        c = css(item);
                    }
                }
                var element = this.createElement(caption(item), c, elementName);
                if (decorator !== undefined) {
                    decorator(item, element);
                }
                elements.push(element);
            }
            return elements;
        };
        Game.prototype.createElement = function (caption, css, elementName) {
            if (elementName === void 0) { elementName = "span"; }
            var element = document.createElement(elementName);
            element.textContent = caption;
            if (css !== undefined) {
                element.className = css;
            }
            return element;
        };
        /**
         * Joins a group of HTML Elements together with a separator span.
         */
        Game.prototype.joinElements = function (elements, separator, separatorClass) {
            var output = [];
            for (var i = 0; i < elements.length; i++) {
                output.push(elements[i]);
                if (i != elements.length - 1) {
                    elements[i].textContent += separator;
                }
            }
            return output;
        };
        Game.prototype.pluralize = function (name, quantity, omitNumber) {
            if (omitNumber === void 0) { omitNumber = false; }
            if (quantity == 1)
                return name;
            var res = "";
            if (!omitNumber) {
                res = StringUtilities.formatNumber(quantity) + " ";
            }
            if (name.charAt(name.length - 1).toLowerCase() === "y")
                return res + name.substr(0, name.length - 1) + "ies";
            if (name.charAt(name.length - 1).toLowerCase() === "h")
                return res + name + "es";
            return res + name + "s";
        };
        Game.prototype.groupItems = function (items) {
            var linq;
            if (Array.isArray(items)) {
                linq = KMud.Linq(items);
            }
            else {
                linq = items;
            }
            return linq.groupBy(function (x) { return x.TemplateId + "|" + x.Name; });
        };
        return Game;
    })();
    KMud.Game = Game;
    var StringUtilities = (function () {
        function StringUtilities() {
        }
        StringUtilities.formatNumber = function (n) {
            return n.toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        };
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
