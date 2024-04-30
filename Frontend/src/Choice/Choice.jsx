import Header from "./Header/Header";
import rostov from "./rostov.png";
import "./Choice.css";
import Footer from "./Footer/Footer";
import Choose from "./Choose/Choose";

export default function Choice() {
  return (
    <>
      <Header />
      <img src={rostov} className="backimg" alt="oblast" />
      <div className="text_ai">
        Искусственный интеллект для Целей устойчивого развития
      </div>
      <div className="podtext">Команда Качаван</div>
      <Choose />
      <Footer />
    </>
  );
}
