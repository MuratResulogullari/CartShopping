import React, { Component } from 'react'
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as cartActions from '../../redux/actions/cartActions';
import { Link } from 'react-router-dom';
class Cart extends Component {
    componentDidMount() {
        this.props.actions.getCartItems();

    }
    renderCart() {
        return (
            <div className='body'>
                <table className='table'>
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Category</th>
                            <th>Quantity</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody className='table-group-divider'>
                        {this.props.cartItems.map((cartItem) => (
                            <tr key={cartItem.Id}>
                                <td>{cartItem.Product.ProductName}</td>
                                <td>{cartItem.Product.Category}</td>
                                <td>{cartItem.Quantity}</td>
                                <td>{cartItem.Product.Price} </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

        )
    }
    renderEmpty() {
        return (
            <div className='container'>
                <div className="alert alert-warning" role="alert">
                    Cart is null
                </div>
            </div >
        )
    }
    render() {
        return (
            <div className='container'>

                <div className='d-flex justify-content-between'>
                    <div><h2>Cart Summary</h2></div>
                </div>
                {this.props.cartItems.length > 0 ? this.renderCart() : this.renderEmpty()}
            </div>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        actions: {
            getCartItems: bindActionCreators(cartActions.getCartItems, dispatch)
        }
    }
};
function mapStateToProps(state) {
    return {
        cartItems: state.cartReducer,
    }
};
export default connect(mapStateToProps, mapDispatchToProps)(Cart);