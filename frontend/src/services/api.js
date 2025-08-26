const getAuthToken = () => {
    return localStorage.getItem('token');
}

const authFetch = async (url, options = {}) => {
    const token = getAuthToken();

    const headers = {
        'Content-Type': 'application/json',
        ...options.headers
    };

    if(token){
        headers.Authorization = `Bearer ${token}`;
    }

    const response = await fetch(url, {
        ...options,
        headers
    });

    if(!response.ok){
        if(response.status === 401){
            // Handle unauthorized access
            console.log('Unauthorized access - redirect to login');
            // Redirect to login page or show login modal
            window.location.href = '/login';
        }
        const errorData = await response.json();
        throw new Error(errorData.message || 'API request failed');
    }

    return response.json();
};

export { authFetch };