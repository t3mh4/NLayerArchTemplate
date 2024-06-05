'use strict';

class message {

    #toastrType = {
        Error: 'error',
        Info: 'info',
        Success: 'success',
        Warning: 'warning'
    };

    get Position() {
        return {
            TopRight: 'toast-top-right',
            TopLeft: 'toast-top-left',
            BottomRight: 'toast-bottom-right',
            BottomLeft: 'toast-bottom-left',
        };
    };

    success = (options) => {
        let { title = 'Başarılı', message = "İşlem Başarılı..!!", position = this.Position.TopRight, timeOut = 3000 } = options || {};
        this.#show(title, message, this.#toastrType.Success, position, timeOut);
    };

    error = (options) => {
        let { title = 'Hata', message = "Beklenmedik bir hata ile karşılaşıldı", position = this.Position.TopRight, timeOut = 3000 } = options || {};
        this.#show(title, message, this.#toastrType.Error, position, timeOut);
    };

    info = ({ title = 'Bilgi', message, position = this.Position.TopRight, timeOut = 3000 }) => {
        this.#show(title, message, this.#toastrType.Info, position, timeOut);
    };

    warning = ({ title = 'Uyarı', message, position = this.Position.TopRight, timeOut = 3000 }) => {
        this.#show(title, message, this.#toastrType.Warning, position, timeOut);
    };

    #show = (title, message, type, position, timeOut) => {
        toastr.options.onHidden = undefined;
        toastr.options.newestOnTop = true;
        toastr.options.positionClass = position;
        toastr.options.progressBar = true;
        toastr.options.timeOut = timeOut;
        toastr.options.onHidden = this.hidden;
        toastr[type](message, title);
    };

    hidden = () => undefined;
}