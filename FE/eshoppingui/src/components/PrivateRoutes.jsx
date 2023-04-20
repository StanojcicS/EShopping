import React from 'react';
import { Route, Navigate } from "react-router-dom";

export const PrivateRoute = ({ component: Component, allowedRoles,  ...props}) => {
    
    const IsAuthenticated = () => {
        const logedInUser = localStorage.getItem("logedInUser");
        return logedInUser != null && logedInUser != undefined;
    }

    const IsAuthorized = () => {
        const logedInUser = JSON.parse(localStorage.getItem("logedInUser"));
        if ( logedInUser == null || logedInUser == undefined)
            return false;
        return allowedRoles.includes(logedInUser.userType);
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