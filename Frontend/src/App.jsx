import "./App.css";
import { Routes, Route, useLocation } from "react-router-dom";
import { useTransition, animated } from "react";
import Choice from "./Choice/Choice";
import AIPage from "./AIPage/AIpage";

function App() {
  const location = useLocation();
  const transitions = useTransition(location, (location) => location.key, {});

  return (
    <>
      <Routes>
        <Route path="/" element={<Choice />} />
        <Route path="/home" element={<Choice />} />
        <Route path="/ai" element={<AIPage />} />
      </Routes>
    </>
  );
}

export default App;
