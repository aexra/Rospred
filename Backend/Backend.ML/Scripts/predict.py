import sys
import pickle

args = sys.argv[1:]

valueid = args[0]
model_name = args[1]
prediction_horizon = int(args[2])

with open(f'../Backend.ML/Models/backend_models.pkl', 'rb') as f:
  models = pickle.load(f)
  
  if model_name == "arima":
    model = models[model_name][valueid]
    forecast = model.forecast(steps=prediction_horizon)
    forecasted_values = forecast.tolist()
  elif model_name == "arch":
    model = models[model_name][valueid]
    forecast = model.forecast(horizon=prediction_horizon)
    forecasted_values = forecast.mean['h.1'].values
  elif model_name == "garch":
    model = models[model_name][valueid]
    forecast = model.forecast(horizon=prediction_horizon)
    forecasted_values = forecast.mean['h.1'].values
  else:
    print("No such model: ", model_name, file=sys.stderr)
    quit(1)
    
  print(forecasted_values)