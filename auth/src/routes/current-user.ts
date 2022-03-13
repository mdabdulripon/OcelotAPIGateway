import express from "express";
import jwt  from "jsonwebtoken";
const router = express.Router();

router.get('/api/users/currentUser', (req, res) => {

    // Check if the jwt is exits
    if (!req.session?.jwt) {
        return res.send({ currentUser: null });
    }

    try {
        const payload = jwt.verify(req.session.jwt, process.env.JWT_KEY!);
        res.send({ currentUser: payload });
    } catch (error) {
        res.send({ currentUser: null }); 
    }
});

export { router as currentUserRouter };
