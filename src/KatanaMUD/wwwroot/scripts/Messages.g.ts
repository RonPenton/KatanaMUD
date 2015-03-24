module KMud {
    export class LoginRejected {
        constructor() { this.MessageName = 'LoginRejected'; }
        public MessageName: string;
        public RejectionMessage: string;
    }
}
