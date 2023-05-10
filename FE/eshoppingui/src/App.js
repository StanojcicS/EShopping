import React from 'react';
import './App.css';
import { Login } from "./components/Login";
import { Register } from "./components/Register";
import { Route, Routes } from "react-router-dom";
import { PrivateRoute } from "./components/PrivateRoutes";
import { MyProfile } from './components/MyProfile';
import { Dashboard } from './components/Dashboard';
import { VerifyUsers } from './components/adminCommands/VerifyUsers';

// window.onbeforeunload = function() {
//   localStorage.clear();
// }

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} /> 
        <Route path="/my-profile" 
               element={<PrivateRoute 
               allowedRoles={["Admin", "Seller", "Buyer"]}  
               component={MyProfile} />} />
        <Route path="/dashboard"
               element= {<PrivateRoute
                          allowedRoles={["Admin", "Seller", "Buyer"]}
                          component={Dashboard} />} />
        <Route path="/verify-users"
               element= {<PrivateRoute
                          allowedRoles={["Admin"]}
                          component={VerifyUsers} />} />
      </Routes>
    </>
  );
}

export default App;
