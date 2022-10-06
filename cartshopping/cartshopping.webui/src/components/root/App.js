import React from 'react';
import './App.css';
import { Routes, Route } from "react-router-dom";
import Header from '../header/Header';
import Home from '../home/Home';
import Cart from '../cart/Cart';
function App() {
    return (
        <>
            <Header />
            <div className="wrapper">
                <Routes>
                    <Route exact path="/" element={<Home />} />
                    <Route exact path="/Cart" element={<Cart />} />
                </Routes>
            </div>
        </>
    );
}

export default App;