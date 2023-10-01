import React, {useEffect, useState} from 'react';
import useAPI from "../CustomHooks/useAPI";
import BookAPI from "../APIs/BookAPI";
import {
    Button,
    ButtonGroup,
    Card, CardActions,
    CardContent,
    CardHeader, Chip,
    FormControl,
    Grid, IconButton,
    InputLabel,
    MenuItem,
    Select,
    Typography
} from "@mui/material";
import RecommendationAPI from "../APIs/RecommendationAPI";
import {useOidc, useOidcAccessToken} from "@axa-fr/react-oidc";
import ProtectedComponentWrapper from "../Utilities/ProtectedComponentWrapper";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import OrderAPI from "../APIs/OrderAPI";


const BookList = ({handleBookModal, handleCategoryModal, refresh}) => {
    const [books, setBooks] = useState([]);
    const [filter, setFilter] = useState('Default');
    const bookAPI = useAPI(BookAPI);
    const recommendationAPI = useAPI(RecommendationAPI);
    const orderAPI = useAPI(OrderAPI);
    const { isAuthenticated } = useOidc();
    const { accessTokenPayload } = useOidcAccessToken();
    const [orders, setOrders] = useState([]);


    const fetchData = async () => {
        let booksData;
        let data
        switch(filter) {
            case 'Popularity':
                data = await recommendationAPI?.getByPopularity(5);
                booksData = data.map(d => d.book);
                break;
            case 'Recommendations':
                data = await recommendationAPI?.getByHistory(3);
                booksData = data.map(d => d.book);
                break;
            default:
                booksData = await bookAPI?.getBooks();
        }
        setBooks(booksData);
        if (isAuthenticated) {
            let orderData = await orderAPI?.getOrders({owner: accessTokenPayload.sub, pageSize: 100})
            setOrders(orderData?.filter(o => !o.isReturned));
        } else {
            setOrders([]);
        }
    };

    useEffect(() => {
        fetchData();
    }, [bookAPI, recommendationAPI, filter, refresh]);

    const handleFilterChange = (event) => {
        setFilter(event.target.value);
    };

    const handleBookDelete = async (book) => {
        await bookAPI.deleteBook(book?.isbn);
        await fetchData();
    }

    const handleBookOrder = async (book) => {
        await orderAPI.createOrder({bookUri: book.isbn});
        await fetchData();
    }

    const handleBookReturnOrder = async (book) => {
        await orderAPI.patchOrder(orders?.find(o => o.bookUri == book.isbn).orderId, {isReturned: true});
        await fetchData();
    }

    return (
        <div>
            <Grid container
                  direction="row"
                  justifyContent="space-between"
                  alignItems="center"
                  spacing={8}
            >
                <Grid item>
                    <FormControl variant="outlined" margin="normal">
                        <InputLabel id="filter-label">Filter</InputLabel>
                        <Select
                            labelId="filter-label"
                            value={filter}
                            onChange={handleFilterChange}
                            label="Filter"
                        >
                            <MenuItem value="Default">Default</MenuItem>
                            <MenuItem value="Popularity">Popularity</MenuItem>
                            {isAuthenticated && <MenuItem value="Recommendations">Recommendations</MenuItem>}
                        </Select>
                    </FormControl>
                </Grid>
                <ProtectedComponentWrapper authorizationRoles={["admin"]}>
                    <Grid item>
                        <ButtonGroup variant="contained" aria-label="outlined primary button group">
                            <Button onClick={() => handleBookModal({})}>ADD BOOK</Button>
                            <Button onClick={() => handleCategoryModal({})}>ADD CATEGORY </Button>
                        </ButtonGroup>
                    </Grid>
                </ProtectedComponentWrapper>
            </Grid>
            <Grid container spacing={4}>
                {books?.map((book, index) => (
                    <Grid item key={index} xs={12} sm={6} md={4}>
                        <Card>
                            <CardHeader
                                title={book?.title}
                                subheader={book.category && <Chip label={book?.category?.name} />}
                                action={
                                    <ProtectedComponentWrapper authorizationRoles={["admin"]}>
                                        <IconButton onClick={() => handleBookModal(book)}><EditIcon/></IconButton>
                                        <IconButton onClick={() => handleBookDelete(book)}><DeleteIcon/></IconButton>
                                    </ProtectedComponentWrapper>
                                }
                            />
                            <CardContent>
                                <Typography variant="body2" color="textSecondary" component="p">
                                    {book?.description}
                                </Typography>
                                <br/>
                                <Typography variant="body2" color="textSecondary" component="p">
                                    Stock: {book?.stock}
                                </Typography>
                                <br/>
                                <Typography variant="body2" color="textSecondary" component="p">
                                    ISBN: {book?.isbn}
                                </Typography>
                            </CardContent>
                            <ProtectedComponentWrapper>
                                {orders?.find(o => o.bookUri === book.isbn) === undefined ?
                                    <CardActions>
                                        <Button size="small" onClick={() => handleBookOrder(book)}>ORDER</Button>
                                    </CardActions>
                                    :
                                    <CardActions>
                                        <Button size="small" onClick={() => handleBookReturnOrder(book)} >RETURN</Button>
                                    </CardActions>
                                }
                            </ProtectedComponentWrapper>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </div>
    );
};

export default BookList;