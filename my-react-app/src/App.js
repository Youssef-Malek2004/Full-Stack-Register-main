import React, { useState, useEffect } from "react";
import "./App.css";
import MyForm from "./Components/myForm";
import CreatingForm from "./Components/creatingForm";

function App() {
  useEffect(() => {
    return () => {};
  }, []);

  return (
    <div className="App">
      <main className="App-main">
        <CreatingForm />
      </main>
      <footer className="App-footer">{/* Your footer content */}</footer>
    </div>
  );
}

export default App;
