module KMud {
    export class Game {
        constructor() {

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
        }

        private addOutput(element: HTMLElement, text: string, css: string = null) {
            var span = document.createElement("span");
            span.textContent = text;
            span.className = css;
            element.appendChild(span);
            var br = document.createElement("br");
            element.appendChild(br);
        }
    }
}