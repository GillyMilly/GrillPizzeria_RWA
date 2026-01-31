(function () {
    var registerUrl = '/api/auth/register';
    var btn = document.getElementById('register-button');

    btn.addEventListener('click', function () {
        var data = {
            username: document.getElementById('username').value.trim(),
            email: document.getElementById('email').value.trim(),
            password: document.getElementById('password').value,
            ime: document.getElementById('ime').value.trim(),
            prezime: document.getElementById('prezime').value.trim(),
            mobitel: document.getElementById('mobitel').value.trim() || null
        };
        if (!data.username || !data.email || !data.password || !data.ime || !data.prezime) {
            alert('Ime, prezime, korisničko ime, e-mail i lozinka su obavezni.');
            return;
        }
        btn.disabled = true;
        fetch(registerUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
            .then(function (r) {
                if (!r.ok) return r.text().then(function (t) { throw new Error(t); });
                return r.json();
            })
            .then(function () {
                alert('Registracija uspješna. Sada se možete prijaviti.');
                window.location.href = '/html/login.html';
            })
            .catch(function (err) {
                alert(err.message || 'Greška pri registraciji.');
                btn.disabled = false;
            });
    });
})();
