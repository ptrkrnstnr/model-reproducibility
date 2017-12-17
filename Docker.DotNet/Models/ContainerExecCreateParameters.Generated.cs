using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ContainerExecCreateParameters // (main.ExecCreateParameters)
    {
        public ContainerExecCreateParameters()
        {
        }

        public ContainerExecCreateParameters(ExecConfig ExecConfig)
        {
            if (ExecConfig != null)
            {
                this.User = ExecConfig.User;
                this.Privileged = ExecConfig.Privileged;
                this.Tty = ExecConfig.Tty;
                this.AttachStdin = ExecConfig.AttachStdin;
                this.AttachStderr = ExecConfig.AttachStderr;
                this.AttachStdout = ExecConfig.AttachStdout;
                this.Detach = ExecConfig.Detach;
                this.DetachKeys = ExecConfig.DetachKeys;
                this.Cmd = ExecConfig.Cmd;
            }
        }

        [DataMember(Name = "User", EmitDefaultValue = false)]
        public string User { get; set; }

        [DataMember(Name = "Privileged", EmitDefaultValue = false)]
        public bool Privileged { get; set; }

        [DataMember(Name = "Tty", EmitDefaultValue = false)]
        public bool Tty { get; set; }

        [DataMember(Name = "AttachStdin", EmitDefaultValue = false)]
        public bool AttachStdin { get; set; }

        [DataMember(Name = "AttachStderr", EmitDefaultValue = false)]
        public bool AttachStderr { get; set; }

        [DataMember(Name = "AttachStdout", EmitDefaultValue = false)]
        public bool AttachStdout { get; set; }

        [DataMember(Name = "Detach", EmitDefaultValue = false)]
        public bool Detach { get; set; }

        [DataMember(Name = "DetachKeys", EmitDefaultValue = false)]
        public string DetachKeys { get; set; }

        [DataMember(Name = "Cmd", EmitDefaultValue = false)]
        public IList<string> Cmd { get; set; }
    }
}
