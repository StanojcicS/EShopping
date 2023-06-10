import { GetAllSellers } from "../../services/UserService";
import React, { useState, useEffect } from 'react';
import {ToastContainer, toast} from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css'; 
import { VerifySeller } from "../../services/UserService";
import { Link } from "react-router-dom";

export const VerifyUsers = () => {
    const [sellers, setSellers] = useState([]);
    const [verificationStatus, setVerificationStatus] = useState(null);
    const token = localStorage.getItem("token");

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    useEffect( () => {
        const getSellers = async () => {
            try{
                const response = await GetAllSellers(handleAlert, token); 
                setSellers(response); 
            }
            catch (ex) {
                console.log(ex);
            }
        };
        getSellers();
    },[]);

    const handleVerifySeller = (username, status, token, handleAlert) => {
        VerifySeller(username, status, token, handleAlert);
        setVerificationStatus(status === 1 ? "Verified" : "Verification request denied");
    }

    return (
        <>
        <ToastContainer/>
        <Link className='link-button' to='/dashboard'>
            <button className="back-to-dashboard-button">Dashboard</button>
        </Link> 
        <h2 style={{color:"white"}}>Sellers verification</h2>
        <table className="verify-sellers-table">
            <tr className="verify-sellers-table-header-row">
                <th>Profile picture</th>
                <th>Username</th>
                <th>Email</th>
                <th>Name</th>
                <th>Last Name</th>
                <th>Date of Birth</th>
                <th>Adress</th>
                <th>Verification status</th>
            </tr>
            {sellers.map(seller => (
                <tr key={seller.username}>
                    <td>
                        <div className="profile-picture-container">
                            <img className="profile-picture" src={seller.profilePictureUrl} alt="No picture"/>
                        </div>
                    </td>
                    <td>{seller.username}</td>
                    <td>{seller.email}</td>
                    <td>{seller.name}</td>
                    <td>{seller.lastName}</td>
                    <td>{seller.dateOfBirth}</td>
                    <td>{seller.adress}</td>
                    <td>
                        {seller.verified === null && 'Verification request denied'}
                        {seller.verified === 1 && 'Verified'}
                        {seller.verified === 0 && (
                        <>
                            {verificationStatus === null ?(
                            <>
                                <button onClick={() => handleVerifySeller(seller.username, 1, token, handleAlert)}>Verify</button>
                                <button onClick={() => handleVerifySeller(seller.username, null, token, handleAlert)}>Decline</button>
                            </>) 
                            : <p>{verificationStatus}</p>}
                        </>
                        )}
                    </td>
                </tr>
            ))}
        </table>
        </>
    );
}