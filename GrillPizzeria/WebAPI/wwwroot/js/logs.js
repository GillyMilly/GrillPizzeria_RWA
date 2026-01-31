(function () {
    var logTableBody = document.querySelector('#logTable tbody');
    var logCountSelect = document.getElementById('logCount');
    var totalCountEl = document.getElementById('totalCount');
    var fetchLogsBtn = document.getElementById('fetchLogs');
    var logoutBtn = document.getElementById('logout');

    function checkAuth() {
        if (!localStorage.getItem('token')) {
            alert('Niste prijavljeni.');
            window.location.href = '/html/login.html';
            return false;
        }
        return true;
    }

    function getToken() {
        return localStorage.getItem('token');
    }

    function fetchCount() {
        if (!checkAuth()) return;
        fetch('/api/logs/count', {
            headers: { 'Authorization': 'Bearer ' + getToken() }
        })
            .then(function (r) {
                if (r.status === 401) {
                    localStorage.removeItem('token');
                    window.location.href = '/html/login.html';
                    return;
                }
                return r.json();
            })
            .then(function (data) {
                if (data && typeof data.count !== 'undefined') {
                    totalCountEl.textContent = 'Ukupno zapisa: ' + data.count;
                }
            })
            .catch(function () {
                totalCountEl.textContent = '';
            });
    }

    function fetchLogs(n) {
        if (!checkAuth()) return;
        n = n || parseInt(logCountSelect.value, 10) || 10;

        fetch('/api/logs/get/' + n, {
            headers: { 'Authorization': 'Bearer ' + getToken() }
        })
            .then(function (response) {
                if (response.status === 401) {
                    localStorage.removeItem('token');
                    window.location.href = '/html/login.html';
                    return null;
                }
                if (!response.ok) {
                    return response.text().then(function (text) {
                        throw new Error(text || 'Greška pri učitavanju.');
                    });
                }
                return response.json();
            })
            .then(function (logs) {
                if (logs == null) return;
                // API može vratiti niz direktno ili objekt s $values (ReferenceHandler.Preserve)
                var list = Array.isArray(logs) ? logs : (logs && logs.$values ? logs.$values : []);
                logTableBody.innerHTML = '';
                if (list.length === 0) {
                    logTableBody.innerHTML = '<tr><td colspan="4">Nema pronađenih zapisnika.</td></tr>';
                    return;
                }
                list.forEach(function (log) {
                    var row = document.createElement('tr');
                    row.innerHTML =
                        '<td>' + (log.id != null ? log.id : '') + '</td>' +
                        '<td>' + (log.timestamp ? new Date(log.timestamp).toLocaleString() : '') + '</td>' +
                        '<td>' + (log.level || '') + '</td>' +
                        '<td>' + (log.message || '') + '</td>';
                    logTableBody.appendChild(row);
                });
            })
            .catch(function (err) {
                logTableBody.innerHTML = '<tr><td colspan="4">Greška: ' + (err.message || 'Učitavanje nije uspjelo.') + '</td></tr>';
            });
    }

    logoutBtn.addEventListener('click', function () {
        localStorage.removeItem('token');
        alert('Odjavljeni ste.');
        window.location.href = '/html/login.html';
    });

    logCountSelect.addEventListener('change', function () {
        fetchLogs(parseInt(this.value, 10));
    });

    fetchLogsBtn.addEventListener('click', function () {
        fetchLogs(parseInt(logCountSelect.value, 10));
    });

    if (checkAuth()) {
        fetchCount();
        fetchLogs(parseInt(logCountSelect.value, 10));
    }
})();
