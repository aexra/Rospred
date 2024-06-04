from pandas import DataFrame
from sklearn.preprocessing import MinMaxScaler
from joblib import load
import numpy as np

def normalize(df: DataFrame) -> (DataFrame, MinMaxScaler):
  scaler = MinMaxScaler()
  df_normalized = df.copy()
  numeric_columns = df.select_dtypes(include=['number']).columns
  df_normalized[numeric_columns] = scaler.fit_transform(df[numeric_columns])
  return (df_normalized, scaler)

def denormalize(label_i: int, forecasted_values: list[float], ncolumns: int):
    scaler = load('../Backend.ML/Models/scaler.pkl')
    n_samples = len(forecasted_values)
    dummy_values = np.zeros((n_samples, ncolumns))
    dummy_values[:, label_i] = forecasted_values
    denormalized = scaler.inverse_transform(dummy_values)
    return denormalized[:, label_i]
