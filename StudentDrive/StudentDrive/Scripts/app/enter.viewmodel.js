function clearError() {
    ViewModel.error("")
}

var ViewModel = {
    login: ko.observable(""),
    password: ko.observable(""),
    error: ko.observable(""),
    onEnter: function () {
        $.ajax({
            type: 'GET',
            url: '/api/Enter/?login=' + ViewModel.login() + "&password=" + ViewModel.password(),
            contentType: 'application/json; charset=UTF-8',
            success: function (data) {
                window.location.replace("/Home/Index/" + data);
            },
            error: function (e) {
                ViewModel.error("Неправильный логин или пароль!");
                setTimeout(clearError, 3000);
            }
        });
    },
    onVkAuth: function () {
        window.location.href = "https://oauth.vk.com/authorize?client_id=&display=page&redirect_uri=http://rose.ddns.net/Home/VkAutorization&scope=friends&response_type=code&v=5.63"
    }
}
$(function () {
    ko.applyBindings(ViewModel);
});