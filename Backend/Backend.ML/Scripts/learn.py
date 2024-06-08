import sys
import os
import pickle
import pandas as pd
import datetime as dt
from contextlib import contextmanager
from statsmodels.tsa.arima.model import ARIMA
from arch import arch_model
import logging

save_path = '../Backend.ML/Models/backend_models.pkl'
args = sys.argv[1:]

pairs = args[0].split("|")

models = {
    "arima": {},
    "arch": {},
    "garch": {}
}

@contextmanager
def suppress_stdout():
    with open(os.devnull, "w") as devnull:
        old_stdout = sys.stdout
        sys.stdout = devnull
        try:  
            yield
        finally:
            sys.stdout = old_stdout

def get_fit_arima(x):
    model = ARIMA(x, order=[len(x) - 1, 2, 0])
    model.initialize_approximate_diffuse()
    result = model.fit()
    return result

def get_fit_arch(x):
    model = arch_model(x, vol='ARCH', p=1, q=1)
    result = model.fit(disp="off")
    return result

def get_fit_garch(x):
    model = arch_model(x, vol='GARCH', p=1, q=1)
    result = model.fit(disp="off")
    return result

counter = 0
for pair in pairs:
    id, value_s = pair.split("&")
    values = list(map(float, value_s.split(",")))
    # print(id, values)
    
    # arima
    arima = get_fit_arima(values)
    # forecast = arima.forecast(steps=5)
    # forecasted_value = forecast.tolist()
    # print("next 5: ", forecasted_value)
    
    # arch
    with suppress_stdout():
        arch = get_fit_arch(values)

    # garch
    with suppress_stdout():    
        garch = get_fit_garch(values)
    
    # save models
    models["arima"][id] = arima
    models["arch"][id] = arch
    models["garch"][id] = garch
    
    counter += 1

with open(save_path, 'wb') as f:
    pickle.dump(models, f)

size = os.path.getsize(save_path)

os.system('cls')
print(f"Done machine learning for {counter} labels. Models are saved ({'{:.2f}'.format((size / 1024 / 1024))} Mb) and ready to work.")
