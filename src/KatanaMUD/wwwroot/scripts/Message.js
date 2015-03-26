var KMud;
(function (KMud) {
    var MessageBase = (function () {
        function MessageBase(MessageName) {
            this.MessageName = MessageName;
        }
        return MessageBase;
    })();
    KMud.MessageBase = MessageBase;
})(KMud || (KMud = {}));
