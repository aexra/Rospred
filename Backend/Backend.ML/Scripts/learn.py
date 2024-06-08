import sys
import os
import pickle
import pandas as pd
import datetime as dt
from statsmodels.tsa.arima.model import ARIMA


save_path = '../Backend.ML/Models/backend_models.pkl'
args = sys.argv[1:]

pairs = args[0].split("|")

models = {
    "arima": {},
    "arch": {},
    "garch": {}
}

def get_fit_arima(x):
    model = ARIMA(x, order=[len(x) - 1, 2, 0])
    model.initialize_approximate_diffuse()
    result = model.fit()
    return result

def get_fit_arch():
    pass

def get_fit_garch():
    pass

counter = 0
for pair in pairs:
    id, value_s = pair.split("&")
    values = list(map(int, value_s.split(",")))
    # print(id, values)
    
    # arima
    arima = get_fit_arima(values)
    # forecast = arima.forecast(steps=5)
    # forecasted_value = forecast.tolist()
    # print("next 5: ", forecasted_value)
    
    # arch
    arch = get_fit_arch()

    # garch
    garch = get_fit_garch()
    
    # save models
    models["arima"][id] = arima
    models["arch"][id] = arch
    models["garch"][id] = garch
    
    counter += 1

with open(save_path, 'wb') as f:
    pickle.dump(models, f)

size = os.path.getsize(save_path)

print(f"Done machine learning for {counter} labels. Models are saved ({'{:.2f}'.format((size / 1024 / 1024))} Mb) and ready to work.")
    