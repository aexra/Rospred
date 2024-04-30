import Header from "../Choice/Header/Header";
import rostov from "../Choice/rostov.png";
import Footer from "../Choice/Footer/Footer";
import Place from "./Place/Place";

export default function AIPage() {
  return (
    <>
      <Header />
      <img src={rostov} className="backimg" alt="oblast" />
      <Place />
      <Footer />
    </>
  );
}
