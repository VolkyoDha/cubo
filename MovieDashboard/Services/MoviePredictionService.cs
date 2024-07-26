using Microsoft.ML;
using MovieDashboard.Models;
using System.Collections.Generic;

namespace MovieDashboard.Services
{
    public class MovieRatingPrediction
    {
        public float Year { get; set; }
        public float PredictedRating { get; set; }
    }

    public class MoviePredictionService
    {
        private readonly MLContext _mlContext;

        public MoviePredictionService()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(IEnumerable<FactMovie> movies)
        {
            var data = _mlContext.Data.LoadFromEnumerable(movies);
            var pipeline = _mlContext.Transforms.Concatenate("Features", "Year")
                .Append(_mlContext.Regression.Trainers.Sdca());

            var model = pipeline.Fit(data);

            _mlContext.Model.Save(model, data.Schema, "MovieRatingModel.zip");
        }

        public float PredictRating(int year)
        {
            var model = _mlContext.Model.Load("MovieRatingModel.zip", out var schema);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MovieRatingPrediction, MovieRatingPrediction>(model);

            var prediction = predictionEngine.Predict(new MovieRatingPrediction { Year = year });

            return prediction.PredictedRating;
        }
    }
}
