
fetch('/api/account/logout', {
    method: 'GET',
    credentials: 'include'
})
    .then(response => {
        if (response.ok) {
            // Remove JWT token from storage
            localStorage.removeItem('jwtToken');
            sessionStorage.removeItem('jwtToken');

            // Redirect to login page
            window.location.href = '/account/login';
        }
        // Handle errors if needed
    })
    .catch(error => console.error('Logout failed:', error));