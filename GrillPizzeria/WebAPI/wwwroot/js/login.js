(function () {
    var loginUrl = '/api/auth/login';
    var loginButton = document.getElementById('login-button');
    var spinner = document.getElementById('spinner-placeholder');

    function showSpinner(show) {
        if (show) {
            spinner.classList.add('spinner');
            loginButton.disabled = true;
        } else {
            spinner.classList.remove('spinner');
            loginButton.disabled = false;
        }
    }

    function doLogin() {
        var emailOrUsername = document.getElementById('emailOrUsername').value.trim();
        var password = document.getElementById('password').value;

        if (!emailOrUsername || !password) {
            alert('Unesite korisničko ime (ili e-mail) i lozinku.');
            return;
        }

        showSpinner(true);

        fetch(loginUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ emailOrUsername: emailOrUsername, password: password })
        })
            .then(function (response) {
                if (!response.ok) {
                    return response.text().then(function (text) {
                        throw new Error(text || 'Neispravno korisničko ime ili lozinka.');
                    });
                }
                return response.json();
            })
            .then(function (data) {
                var token = data.token;
                if (token) {
                    localStorage.setItem('token', token);
                    window.location.href = '/html/logs.html';
                } else {
                    alert('Odgovor nije sadržavao token.');
                    showSpinner(false);
                }
            })
            .catch(function (err) {
                alert(err.message || 'Greška pri prijavi.');
                localStorage.removeItem('token');
                showSpinner(false);
            });
    }

    loginButton.addEventListener('click', doLogin);
})();
