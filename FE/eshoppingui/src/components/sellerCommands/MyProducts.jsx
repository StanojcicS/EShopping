import React, { useState, useEffect } from 'react';
import {ToastContainer, toast} from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css';
import { GetSellersProducts } from '../../services/ProductService';
import { DeleteProduct } from '../../services/ProductService';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

export const MyProducts = () => {
    const [products, setProducts] = useState([]);
    const token = localStorage.getItem("token");
    const navigate = useNavigate();

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    const handleUpdateProduct = async (id,
        name,
        price,
        quantity,
        description,
        image) => 
    {
          navigate("/update-product",{
            state:{
                pId:id,
                pName:name,
                pPrice:price,
                pQuantity:quantity,
                pDescription:description,
                pImage:image
            }
          })
    }


    const handleDeleteProduct = async (productId) => {
        await DeleteProduct(handleAlert, token, productId);
        window.location.reload();
    }

    useEffect( () => {
        const getProducts = async () => {
            try{
                const response = await GetSellersProducts(handleAlert,token);
                setProducts(response);
            }
            catch (ex){
                console.log(ex);
            }
        };
        getProducts();
    }, []);

    return (
        <>
        <ToastContainer/>
        <h2 style={{color:"white"}}>My products</h2>
        {products.length === 0 ?
            <p style={{color:"white"}}>You don't have any products, please add some</p>
            :
            <table className='verify-sellers-table'>
                <tr className="verify-sellers-table-header-row">
                    <th style={{display:"none"}}>Id</th>
                    <th>Image</th>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Description</th>
                    <th></th>
                </tr>
                {products.map(product => (
                    <tr key={product.id}>
                        <td style={{display:"none"}}>{product.id}</td>
                        <td>
                            <img className='profile-picture' src={product.image} alt="No picture" />    
                        </td>
                        <td>{product.name}</td>  
                        <td>{product.price}</td>
                        <td>{product.quantity}</td>
                        <td>{product.description}</td>
                        <td>
                            <>
                                <button onClick = {() => handleUpdateProduct(product.id,
                                                                            product.name,
                                                                            product.price,
                                                                            product.quantity,
                                                                            product.description,
                                                                            product.image)}>
                                                                                Update</button>
                                <button onClick = {() => handleDeleteProduct(product.id)}>Delete</button>
                            </>    
                        </td>
                    </tr>
                ))}
            </table>
        }
        <Link className='link-button' to='/add-new-product'>
            <button className="add-new-product-button">Add new product</button>
        </Link>
        </>
    )
}