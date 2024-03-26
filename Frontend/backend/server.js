const express = require("express");
const cors = require('cors')
const app = express();
app.use(cors())
const fs = require("fs");
let counter = 0
app.get("/video/:id", function (req, res) {
    setTimeout(() => {
        const contentId = req.params.id;
        const range = req.headers.range;
        if (!range) {
            res.status(400).send("Requires Range header");
            return;
        }
        const videoPath = "out.mp4";
        const videoSize = fs.statSync("out.mp4").size;
        const CHUNK_SIZE = 4 * 10 ** 6;
        const start = Number(range.replace(/\D/g, ""));
        const end = Math.min(start + CHUNK_SIZE, videoSize - 1);
        const contentLength = end - start + 1;
        const headers = {
            "Content-Range": `bytes ${start}-${end}/${videoSize}`,
            "Accept-Ranges": "bytes",
            "Content-Length": contentLength,
            "Content-Type": "video/mp4",
        };
        console.log(headers)
        res.writeHead(206, headers);
        const videoStream = fs.createReadStream(videoPath, { start, end });
        videoStream.pipe(res);
    },2000)
});

app.listen(8030, function () {
    console.log("Listening on port 8030!");
});