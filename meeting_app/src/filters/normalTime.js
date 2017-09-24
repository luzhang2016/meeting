export const normalTime = (time) => {
    if (time) {
        var e = new Date();

        e.setTime(time);

        let y = e.getFullYear();
        // var m = oDate.getMonth() + 1;
        // var d = oDate.getDate();

        // var h = oDate.getHours();
        // var min = oDate.getMinutes();

        // let m = (e.getMonth() + 1) < 10 ? "0" + (e.getMonth() + 1) : (e.getMonth() + 1);
        let m = (e.getMonth() + 1).padStart(2, 0);
        // let d = e.getDate() < 10 ? "0" + e.getDate() : e.getDate();
        let d = e.getDate().padStart(2, 0);
        let h = e.getHours() < 10 ? "0" + e.getHours() : e.getHours();
        let min = e.getMinutes() < 10 ? "0" + e.getMinutes() : e.getMinutes();
        let s = e.getSeconds() < 10 ? "0" + e.getSeconds() : e.getSeconds();

        return y + '-' + m + '-' + d
    }
}