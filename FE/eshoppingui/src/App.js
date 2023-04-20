import React from 'react';
import './App.css';
import { Login } from "./components/Login";
import { Register } from "./components/Register";
import { Route, Routes } from "react-router-dom";
import { PrivateRoute } from "./components/PrivateRoutes";
import { MyProfile } from './components/MyProfile';


function App() {
  return (
    <>
      <Routes>
        <Route path='/' element={<Login />} />
        <Route path='/register' element={<Register />} />
        <Route path='/login' element={<Login />} />
        <Route path='/my-profile' 
               element={<PrivateRoute 
               allowedRoles={["Admin", "Seller", "Buyer"]}  
               component={MyProfile} />} />
      </Routes>
    </>
  );
}

export default App;
