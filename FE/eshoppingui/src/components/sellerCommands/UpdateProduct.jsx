import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { UpdateProduct } from '../../services/ProductService';

export const UpdateProductForm = () => {
    const token = localStorage.getItem("token");
    const navigate = useNavigate();
    const location = useLocation();
    const { pId, pName, pPrice, pQuantity, pDescription, pImage } = location.state;
    const [id, setId] = useState(pId);
    const [name, setName] = useState(pName);
    const [price, setPrice] = useState(pPrice);
    const [quantity, setQuantity] = useState(pQuantity);
    const [description, setDescription] = useState(pDescription);
    const [image, setImage] = useState(pImage);
    const [updateImage, setUpdatedImage] = useState(null);

    const handleAlert = (message, type) => {
        if(type === "success")
            toast.success(message);
        else
            toast.error(message);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        await UpdateProduct(handleAlert, navigate, token, id, name, price, quantity, description, updateImage);
    };

    return(
        <div className="auth-form-container">
            <ToastContainer/>
            <h2>Update product</h2>
            <form className="update-product-form" onSubmit={handleSubmit}>
                <label htmlFor="name">name</label>
                <input value={name} onChange={(e) => setName(e.target.value)} type="text" placeholder="name" />
                
                <label htmlFor="price">price</label>
                <input value={price} onChange={(e) => setPrice(e.target.value)} type="number" placeholder="product price" />

                <label htmlFor="quantity">quantity</label>
                <input value={quantity} onChange={(e) => setQuantity(e.target.value)} type="number" placeholder="quantity of product in stock" onKeyPress={(event) => {
                                    if (!/[0-9]/.test(event.key)) {
                                    event.preventDefault();
                                    }}}/>

                <label htmlFor="description">description</label>
                <input value={description} onChange={(e) => setDescription(e.target.value)} type="description" placeholder="product description" />

                <label htmlFor="image">image</label>
                <img className='profile-picture' src={image} alt='product'/>
                <input type="file" accept="image/*" onChange={(e) => setUpdatedImage(e.target.files[0])} id="updateImage" name="updateImage" />
                                
                <button type="submit">Update</button>
            </form>
        </div>
    )

}