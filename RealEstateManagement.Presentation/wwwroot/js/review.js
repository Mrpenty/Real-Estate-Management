async function addComment(dto) {
    try {
        const token = localStorage.getItem('authToken');
        if (!token) window.location.href = '/Auth/Login';
        const response = await fetch(`https://localhost:7031/api/Review/add`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(dto)
        });

        const data = await response.json();
        //$(`#heart-icon-${id}`).attr('fill', 'none');
        console.log(data);

        return data;
    } catch (error) {
        console.error('Update error:', error);
        throw error;
    }
}