/// <reference path="jquery.d.ts" />

module KMud {
    export class Game {
        private input: HTMLInputElement;
        private lastCommands: string[] = [];
        private currentCommand: number = -1;

        constructor() {
            this.input = <HTMLInputElement>document.getElementById("InputBox");
            $("#InputBox").keypress(x => {
                if (x.keyCode == 13 && this.input.value.trim() != "") {
                    this.addOutput(document.getElementById("Output"), this.input.value, "command-text");
                    this.lastCommands.unshift(this.input.value);
                    this.processCommand(this.input.value);
                    this.input.value = "";
                    this.currentCommand = -1;

                    while (this.lastCommands.length > 20) {
                        this.lastCommands.shift()
                    }
                }
            }).keydown(x => {
                if (x.keyCode == 38 && x.ctrlKey) {
                    if (this.currentCommand < this.lastCommands.length - 1) {
                        this.currentCommand++;
                        this.input.value = this.lastCommands[this.currentCommand];
                    }
                }
                else if (x.keyCode == 40 && x.ctrlKey) {
                    if (this.currentCommand >= 1) {
                        this.currentCommand--;
                        this.input.value = this.lastCommands[this.currentCommand];
                    }
                    else if (this.currentCommand == 0) {
                        this.input.value = "";
                        this.currentCommand = -1;
                    }
                }

            });;
            $(window).mouseup(x => {
                if (window.getSelection().toString().length == 0)
                    $("#InputBox").focus();
            });
        }

        private processCommand(command: string) {
            var lower = command.toLocaleLowerCase().trim();
            var words = command.split(/\s+/gi);
            if (words[0] == "ping") {
                var message = new PingMessage();
                message.SendTime = new Date();
                this._socket.send(JSON.stringify(message));
            }
        }

        private _socket: WebSocket;

        public connect(url: string) {
            this._socket = new WebSocket(url, "kmud");
            this._socket.onopen = e => this.onopen(e);
            this._socket.onclose = e => this.onclose(e);
            this._socket.onerror = e => this.onerror(e);
            this._socket.onmessage = e => this.onmessage(e);
        }

        public SendMessage(message: MessageBase) {
            if (this._socket) {
                this._socket.send(JSON.stringify(message));
            }
        }

        private onopen(e: Event) {

        }
        private onclose(e: Event) {
            this.addOutput(document.getElementById("Output"), "Disconnected", "error-text");
        }
        private onerror(e: Event) {
        }
        private onmessage(e: Event) {
            var message: MessageBase = JSON.parse((<any>e).data);

            if (message.MessageName == LoginRejected.ClassName) {
                this.addOutput(document.getElementById("Output"),(<LoginRejected>message).RejectionMessage, "error-text");
            }
            if (message.MessageName == ServerMessage.ClassName) {
                this.addOutput(document.getElementById("Output"),(<ServerMessage>message).Contents);
            }
            if (message.MessageName == PongMessage.ClassName) {
                var latency = new Date().getMilliseconds() - new Date((<any>message).SendTime).getMilliseconds();
                this.addOutput(document.getElementById("Output"), "Ping: Latency " + latency + "ms", "system-text");
            }
        }

        private addOutput(element: HTMLElement, text: string, css: string = null) {

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
        }
    }
}