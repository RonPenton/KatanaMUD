var KMud;
(function (KMud) {
    var LoginRejected = (function () {
        function LoginRejected() {
            this.MessageName = "LoginRejected";
        }
        return LoginRejected;
    })();
    KMud.LoginRejected = LoginRejected;
    (function (TestEnum) {
        TestEnum[TestEnum["OK"] = 0] = "OK";
        TestEnum[TestEnum["Noo"] = 4] = "Noo";
        TestEnum[TestEnum["Test"] = 6] = "Test";
    })(KMud.TestEnum || (KMud.TestEnum = {}));
    var TestEnum = KMud.TestEnum;
})(KMud || (KMud = {}));
