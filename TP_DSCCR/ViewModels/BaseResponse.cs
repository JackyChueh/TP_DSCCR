using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.ViewModels
{
    public class Result
    {
        private ResultEnum _state = ResultEnum.FAIL;

        public ResultEnum State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                this.Msg = EnumEx.GetDescription(_state);
                //this.Msg = EnumEx.GetDescription((ResultEnum)value);
            }
        }

        public string Msg { get; set; }
    }

    public class BaseResponse
    {
        public Result Result { get; set; }

        public BaseResponse()
        {
            this.Result = new Result();
        }
    }
}