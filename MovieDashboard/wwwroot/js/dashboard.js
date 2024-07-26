function loadChartData() {
    fetch('/api/movies/ratings')
        .then(response => response.json())
        .then(data => {
            var ctx = document.getElementById('ratingChart').getContext('2d');
            var ratingChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: 'IMDB Ratings',
                        data: data.ratings,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Error fetching the data:', error));
}

// Inicializa la carga de datos al cargar la página
document.addEventListener('DOMContentLoaded', function () {
    loadChartData();
});
