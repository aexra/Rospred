import sys
import pickle
import normalizer

args = sys.argv[1:]

prediction_name = args[0]
prediction_horizon = int(args[1])

with open('models/trained_models.pkl', 'rb') as f:
  models = pickle.load(f)
  model = models[prediction_name]
  forecast = model.forecast(steps=prediction_horizon)
  forecasted_values = forecast.values.tolist()
  denormalized = normalizer.denormalize(forecasted_values)
  print(denormalized)