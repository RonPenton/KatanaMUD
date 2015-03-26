var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var KMud;
(function (KMud) {
    var LoginRejected = (function (_super) {
        __extends(LoginRejected, _super);
        function LoginRejected() {
            _super.call(this, 'LoginRejected');
        }
        LoginRejected.ClassName = 'LoginRejected';
        return LoginRejected;
    })(KMud.MessageBase);
    KMud.LoginRejected = LoginRejected;
    var ServerMessage = (function (_super) {
        __extends(ServerMessage, _super);
        function ServerMessage() {
            _super.call(this, 'ServerMessage');
        }
        ServerMessage.ClassName = 'ServerMessage';
        return ServerMessage;
    })(KMud.MessageBase);
    KMud.ServerMessage = ServerMessage;
})(KMud || (KMud = {}));
