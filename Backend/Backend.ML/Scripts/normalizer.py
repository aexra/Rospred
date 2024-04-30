from pandas import DataFrame
from sklearn.preprocessing import MinMaxScaler

def normalize(df: DataFrame) -> (DataFrame, MinMaxScaler):
  scaler = MinMaxScaler()
  df_normalized = df.copy()
  numeric_columns = df.select_dtypes(include=['number']).columns
  df_normalized[numeric_columns] = scaler.fit_transform(df[numeric_columns])
  return (df_normalized, scaler)

def denormalize(forecasted_values):
    denormalized = {}
    for model, columns in forecasted_values.items():
        denormalized[model] = {}
        frame = DataFrame.from_dict(columns)
        for row in frame.iterrows():
            derow = scaler.inverse_transform(DataFrame([row[1]]))
            for i, name in enumerate(row[1].index):
                if not name in denormalized[model].keys():
                    denormalized[model][name] = []
                denormalized[model][name].append(derow[0][i])
    return denormalized
