module KMud {
    export class LoginRejected extends MessageBase {
        constructor() { super('LoginRejected'); }
        public RejectionMessage: string;
        public static ClassName: string = 'LoginRejected';
    }
    export class ServerMessage extends MessageBase {
        constructor() { super('ServerMessage'); }
        public Contents: string;
        public static ClassName: string = 'ServerMessage';
    }
}
