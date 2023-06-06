import React from 'react';
import { Route, Navigate, useLocation } from "react-router-dom";

export const PrivateRoute = ({ component: Component, allowedRoles, ...props}) => {
    const location = useLocation();
    const IsAuthenticated = () => {
        const logedInUser = localStorage.getItem("logedInUser");
        return logedInUser != null && logedInUser != undefined;
    }

    const IsAuthorized = () => {
        const logedInUser = JSON.parse(localStorage.getItem("logedInUser"));
        if ( logedInUser == null || logedInUser == undefined)
            return false;

        if (allowedRoles && !allowedRoles.includes(logedInUser.userType)) {
            return false;
        }

        if (location.pathname === '/my-profile') {
            return true;  
        }
        if (location.pathname === '/dashboard') {
            return true;  
        }
        if (logedInUser.userType === 'Seller' && logedInUser.verified !== 'Your account is verified.') {
            return false;
        }
        return true;
    }

    if(!IsAuthenticated())
    {
        return (
            <Navigate to="/login" /> 
        );
    }

    
    if(allowedRoles && !IsAuthorized())
    { 
        return (
            <Navigate to="/my-profile"/>
        );
    }

    return (
        <React.Fragment>
            <Component {...props} />
        </React.Fragment>
    );
} 