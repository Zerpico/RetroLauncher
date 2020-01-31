using System.Collections;
using System.ComponentModel;

namespace RetroLauncher.DesktopClient.Service {
    // Функции ShouldSerialize<PropertyName> используются обозревателем свойств VS чтобы проверить, должно ли конкретное свойство быть сериализовано. Эти функции добавляются для всех свойств ValueType (свойства типа Int32, BOOL и т.д., для которых не может быть установлено значение null). Эти функции используют функцию Is<PropertyName>Null. Также эти функции используются в реализации TypeConverter для свойств для проверки значения NULL свойства, чтобы пустое значение могло быть отображено в обозревателе свойств для перетаскивания и копирования в Visual studio.
    // Функции Is<PropertyName>Null() используются, чтобы проверить, имеет ли свойство значение NULL.
    // Функции Reset<PropertyName> добавлены для свойств чтения/записи Nullable. Эти функции используются VS designer в обозревателе свойств, чтобы установить для свойства значение NULL.
    // Каждое свойство, добавленное к классу для свойства WMI, имеет набор атрибутов, который определяет его поведение в Visual Studio designer, а также определяет используемый TypeConverter.
    // Функции преобразования даты и времени ToDateTime и ToDmtfDateTime добавлены к классу для преобразования даты и времени DMTF в System.DateTime и наоборот.
    // Класс Early Bound создан для класса WMI.Win32_Process
    public class WIN32_Process : System.ComponentModel.Component {

        // Частное свойство, которое хранит пространство имен WMI, в котором находится этот класс.
        private static string CreatedWmiNamespace = "root\\cimv2";

        // Частное свойство, которое хранит имя класса WMI, создавшее этот класс.
        private static string CreatedClassName = "Win32_Process";

        // Частное поле для хранения ManagementScope, используемое различными методами.
        private static System.Management.ManagementScope statMgmtScope = null;

        private ManagementSystemProperties PrivateSystemProperties;

        // Зависимый объект WMI lateBound.
        private System.Management.ManagementObject PrivateLateBoundObject;

        // Поле для хранения поведения "автоматической отправки" класса.
        private bool AutoCommitProp;

        // Частная переменная, которая хранит встроенное свойство, соответствующее экземпляру.
        private System.Management.ManagementBaseObject embeddedObj;

        // Текущий используемый объект WMI
        private System.Management.ManagementBaseObject curObj;

        // Флаг, указывающий, является ли экземпляр встроенным объектом.
        private bool isEmbedded;

        // Ниже приводятся различные перезагрузки конструкторов для инициализации экземпляра класса с объектом WMI.
        public WIN32_Process() {
            this.InitializeObject(null, null, null);
        }

        public WIN32_Process(string keyHandle) {
            this.InitializeObject(null, new System.Management.ManagementPath(WIN32_Process.ConstructPath(keyHandle)), null);
        }

        public WIN32_Process(System.Management.ManagementScope mgmtScope, string keyHandle) {
            this.InitializeObject(((System.Management.ManagementScope)(mgmtScope)), new System.Management.ManagementPath(WIN32_Process.ConstructPath(keyHandle)), null);
        }

        public WIN32_Process(System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(null, path, getOptions);
        }

        public WIN32_Process(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path) {
            this.InitializeObject(mgmtScope, path, null);
        }

        public WIN32_Process(System.Management.ManagementPath path) {
            this.InitializeObject(null, path, null);
        }

        public WIN32_Process(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(mgmtScope, path, getOptions);
        }

        public WIN32_Process(System.Management.ManagementObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                PrivateLateBoundObject = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
                curObj = PrivateLateBoundObject;
            }
            else {
                throw new System.ArgumentException("Имя класса не совпадает.");
            }
        }

        public WIN32_Process(System.Management.ManagementBaseObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                embeddedObj = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(theObject);
                curObj = embeddedObj;
                isEmbedded = true;
            }
            else {
                throw new System.ArgumentException("Имя класса не совпадает.");
            }
        }

        // Свойство возвращает пространство имен класса WMI.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace {
            get {
                return "root\\cimv2";
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName {
            get {
                string strRet = CreatedClassName;
                if ((curObj != null)) {
                    if ((curObj.ClassPath != null)) {
                        strRet = ((string)(curObj["__CLASS"]));
                        if (((strRet == null)
                                    || (strRet == string.Empty))) {
                            strRet = CreatedClassName;
                        }
                    }
                }
                return strRet;
            }
        }

        // Свойство, указывающее встроенный объект для свойств get System объекта WMI.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties {
            get {
                return PrivateSystemProperties;
            }
        }

        // Свойство, возвращающее объект lateBound.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementBaseObject LateBoundObject {
            get {
                return curObj;
            }
        }

        // ManagementScope объекта.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementScope Scope {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Scope;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    PrivateLateBoundObject.Scope = value;
                }
            }
        }

        // Свойство, которое отображает поведение выделения для объекта WMI. Если оно имеет значение "истина", объект WMI будет автоматически сохраняться после любого изменения свойства. (Т.е. после изменения свойства вызывается Put()).
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit {
            get {
                return AutoCommitProp;
            }
            set {
                AutoCommitProp = value;
            }
        }

        // ManagementPath зависимого объекта WMI.
        [Browsable(true)]
        public System.Management.ManagementPath Path {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Path;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    if ((CheckIfProperClass(null, value, null) != true)) {
                        throw new System.ArgumentException("Имя класса не совпадает.");
                    }
                    PrivateLateBoundObject.Path = value;
                }
            }
        }

        // Свойство общей статической области, используемое различными методами.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static System.Management.ManagementScope StaticScope {
            get {
                return statMgmtScope;
            }
            set {
                statMgmtScope = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Caption {
            get {
                return ((string)(curObj["Caption"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство CommandLine указывает командную строку, используемую для запуска процесс" +
            "а, если таковая используется.")]
        public string CommandLine {
            get {
                return ((string)(curObj["CommandLine"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство CreationClassName указывает имя класса или подкласса, используемого в со" +
            "здании экземпляра. При использовании с другими ключевыми свойствами данного клас" +
            "са это свойство позволяет уникально идентифицировать все экземпляры класса и его" +
            " подклассов.")]
        public string CreationClassName {
            get {
                return ((string)(curObj["CreationClassName"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCreationDateNull {
            get {
                if ((curObj["CreationDate"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Время начала выполнения процесса.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime CreationDate {
            get {
                if ((curObj["CreationDate"] != null)) {
                    return ToDateTime(((string)(curObj["CreationDate"])));
                }
                else {
                    return System.DateTime.MinValue;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("CSCreationClassName содержит имя класса создания компьютерной системы.")]
        public string CSCreationClassName {
            get {
                return ((string)(curObj["CSCreationClassName"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Имя компьютерной системы.")]
        public string CSName {
            get {
                return ((string)(curObj["CSName"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Description {
            get {
                return ((string)(curObj["Description"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство ExecutablePath указывает путь к исполняемому файлу процесса.\nПример: C:\\" +
            "WINDOWS\\EXPLORER.EXE")]
        public string ExecutablePath {
            get {
                return ((string)(curObj["ExecutablePath"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsExecutionStateNull {
            get {
                if ((curObj["ExecutionState"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Указывает текущее рабочее состояние процесса. Примеры возможных значений: готов (" +
            "2), выполняется (3), заблокирован (4).")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ExecutionStateValues ExecutionState {
            get {
                if ((curObj["ExecutionState"] == null)) {
                    return ((ExecutionStateValues)(System.Convert.ToInt32(10)));
                }
                return ((ExecutionStateValues)(System.Convert.ToInt32(curObj["ExecutionState"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Строка, используемая для идентификации процесса. ИД процесса — это тип дескриптор" +
            "а процесса.")]
        public string Handle {
            get {
                return ((string)(curObj["Handle"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHandleCountNull {
            get {
                if ((curObj["HandleCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство HandleCount указывает общее число дескрипторов, открытых в настоящее время процессом. Это число представляет собой количество дескрипторов, открытых во всех потоках в процессе. Дескриптор используется для работы с системными ресурсами. Каждому дескриптору соответствует запись в таблице дескрипторов. Эти записи содержат адреса ресурсов и средства для определения типа ресурсов.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint HandleCount {
            get {
                if ((curObj["HandleCount"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["HandleCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallDateNull {
            get {
                if ((curObj["InstallDate"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime InstallDate {
            get {
                if ((curObj["InstallDate"] != null)) {
                    return ToDateTime(((string)(curObj["InstallDate"])));
                }
                else {
                    return System.DateTime.MinValue;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsKernelModeTimeNull {
            get {
                if ((curObj["KernelModeTime"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Время в режиме ядра, в 100 наносекундах. Если эта информация отсутствует, использ" +
            "уется значение 0.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong KernelModeTime {
            get {
                if ((curObj["KernelModeTime"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["KernelModeTime"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMaximumWorkingSetSizeNull {
            get {
                if ((curObj["MaximumWorkingSetSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство MaximumWorkingSetSize указывает максимальный размер рабочего набора процесса. Рабочий набор процесса — это набор страниц, доступных процессу в физическом ОЗУ. Эти страницы являются резидентными, доступны приложению постоянно и не вызывают ошибку страницы.
Пример: 1413120.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MaximumWorkingSetSize {
            get {
                if ((curObj["MaximumWorkingSetSize"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["MaximumWorkingSetSize"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMinimumWorkingSetSizeNull {
            get {
                if ((curObj["MinimumWorkingSetSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство MinimumWorkingSetSize указывает минимальный размер рабочего набора процесса. Рабочий набор процесса — это набор страниц, доступных процессу в физическом ОЗУ. Эти страницы являются резидентными, доступны приложению постоянно и не вызывают ошибку страницы.
Пример: 20480.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MinimumWorkingSetSize {
            get {
                if ((curObj["MinimumWorkingSetSize"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["MinimumWorkingSetSize"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name {
            get {
                return ((string)(curObj["Name"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Имя класса создания операционной системы.")]
        public string OSCreationClassName {
            get {
                return ((string)(curObj["OSCreationClassName"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Имя операционной системы.")]
        public string OSName {
            get {
                return ((string)(curObj["OSName"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsOtherOperationCountNull {
            get {
                if ((curObj["OtherOperationCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство OtherOperationCount указывает число выполненных операций ввода-вывода, о" +
            "тличных от операций чтения и записи.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong OtherOperationCount {
            get {
                if ((curObj["OtherOperationCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["OtherOperationCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsOtherTransferCountNull {
            get {
                if ((curObj["OtherTransferCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство OtherTransferCount указывает размер данных, переданных в процессе операц" +
            "ий, отличных от операций ввода-вывода.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong OtherTransferCount {
            get {
                if ((curObj["OtherTransferCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["OtherTransferCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPageFaultsNull {
            get {
                if ((curObj["PageFaults"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство PageFaults указывает число ошибок страниц, созданных процессом.\nПример: " +
            "10")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint PageFaults {
            get {
                if ((curObj["PageFaults"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["PageFaults"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPageFileUsageNull {
            get {
                if ((curObj["PageFileUsage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство PageFileUsage указывает размер части файла подкачки, используемой процес" +
            "сом в настоящее время.\nПример: 102435")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint PageFileUsage {
            get {
                if ((curObj["PageFileUsage"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["PageFileUsage"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsParentProcessIdNull {
            get {
                if ((curObj["ParentProcessId"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство ParentProcessId указывает уникальный идентификатор процесса, создавшего этот процесс. Идентификаторы процессов могут использоваться повторно, поэтому они актуальны только в течение времени существования процесса. Определяемый свойством ParentProcessId процесс может быть прерван, поэтому свойство может указывать не на работающий процесс. Также возможна ситуация, когда ParentProcessId некорректно ссылается на другой процесс, повторно использующий идентификатор. На основании значения свойства CreationDate можно определить, был ли родительский процесс создан позже данного процесса.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ParentProcessId {
            get {
                if ((curObj["ParentProcessId"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ParentProcessId"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPeakPageFileUsageNull {
            get {
                if ((curObj["PeakPageFileUsage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство PeakPageFileUsage указывает максимальный размер части файла подкачки, ис" +
            "пользованной процессом за время жизни.\nПример: 102367")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint PeakPageFileUsage {
            get {
                if ((curObj["PeakPageFileUsage"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["PeakPageFileUsage"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPeakVirtualSizeNull {
            get {
                if ((curObj["PeakVirtualSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство PeakVirtualSize указывает максимальное значение размера виртуального адресного пространства, использованного процессом единовременно. Использование виртуального адресного пространства не означает обязательного использования соответствующего пространства на диске или в оперативной памяти. Однако виртуальное пространство имеет конечные размеры, и при чрезмерном использовании процесс может ограничить возможность загрузки библиотек.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong PeakVirtualSize {
            get {
                if ((curObj["PeakVirtualSize"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["PeakVirtualSize"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPeakWorkingSetSizeNull {
            get {
                if ((curObj["PeakWorkingSetSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство PeakWorkingSetSize указывает пиковое значение размера рабочего набора пр" +
            "оцесса.\nПример: 1413120")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint PeakWorkingSetSize {
            get {
                if ((curObj["PeakWorkingSetSize"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["PeakWorkingSetSize"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPriorityNull {
            get {
                if ((curObj["Priority"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство Priority указывает назначенный процессу приоритет в операционной системе" +
            ".  Чем больше это значение, тем более высокий приоритет имеет процесс. Допустимы" +
            "й диапазон значений: от 0 (самый низкий приоритет) до 31 (самый высокий).\nПример" +
            ": 7.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint Priority {
            get {
                if ((curObj["Priority"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["Priority"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPrivatePageCountNull {
            get {
                if ((curObj["PrivatePageCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство PrivatePageCount указывает текущее число страниц, доступных только этому" +
            " процессу ")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong PrivatePageCount {
            get {
                if ((curObj["PrivatePageCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["PrivatePageCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsProcessIdNull {
            get {
                if ((curObj["ProcessId"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство ProcessId содержит глобальный идентификатор процесса, с помощью которого" +
            " можно идентифицировать процесс. Значение этого свойства актуально с момента соз" +
            "дания процесса до окончания его работы.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ProcessId {
            get {
                if ((curObj["ProcessId"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ProcessId"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsQuotaNonPagedPoolUsageNull {
            get {
                if ((curObj["QuotaNonPagedPoolUsage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство QuotaNonPagedPoolUsage указывает размер используемой процессом части нев" +
            "ыгружаемого пула.\nПример: 15")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint QuotaNonPagedPoolUsage {
            get {
                if ((curObj["QuotaNonPagedPoolUsage"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["QuotaNonPagedPoolUsage"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsQuotaPagedPoolUsageNull {
            get {
                if ((curObj["QuotaPagedPoolUsage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство QuotaPagedPoolUsage указывает размер используемой процессом части выгруж" +
            "аемого пула.\nПример: 22")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint QuotaPagedPoolUsage {
            get {
                if ((curObj["QuotaPagedPoolUsage"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["QuotaPagedPoolUsage"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsQuotaPeakNonPagedPoolUsageNull {
            get {
                if ((curObj["QuotaPeakNonPagedPoolUsage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство QuotaPeakNonPagedPoolUsage указывает пиковое значение размера используем" +
            "ой процессом части невыгружаемого пула.\nПример: 31")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint QuotaPeakNonPagedPoolUsage {
            get {
                if ((curObj["QuotaPeakNonPagedPoolUsage"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["QuotaPeakNonPagedPoolUsage"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsQuotaPeakPagedPoolUsageNull {
            get {
                if ((curObj["QuotaPeakPagedPoolUsage"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство QuotaPeakPagedPoolUsage указывает пиковое значение размера используемой " +
            "процессом части выгружаемого пула.\nПример: 31")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint QuotaPeakPagedPoolUsage {
            get {
                if ((curObj["QuotaPeakPagedPoolUsage"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["QuotaPeakPagedPoolUsage"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsReadOperationCountNull {
            get {
                if ((curObj["ReadOperationCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство ReadOperationCount указывает число выполненных операций чтения.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong ReadOperationCount {
            get {
                if ((curObj["ReadOperationCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["ReadOperationCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsReadTransferCountNull {
            get {
                if ((curObj["ReadTransferCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство ReadTransferCount указывает размер прочитанных данных.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong ReadTransferCount {
            get {
                if ((curObj["ReadTransferCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["ReadTransferCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSessionIdNull {
            get {
                if ((curObj["SessionId"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство SessionId указывает уникальный идентификатор, который генерируется опера" +
            "ционной системой при создании сеанса. Сеанс представляет собой промежуток времен" +
            "и между входом и выходом из системы.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint SessionId {
            get {
                if ((curObj["SessionId"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["SessionId"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Status {
            get {
                return ((string)(curObj["Status"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTerminationDateNull {
            get {
                if ((curObj["TerminationDate"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Время остановки или прекращения процесса.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime TerminationDate {
            get {
                if ((curObj["TerminationDate"] != null)) {
                    return ToDateTime(((string)(curObj["TerminationDate"])));
                }
                else {
                    return System.DateTime.MinValue;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsThreadCountNull {
            get {
                if ((curObj["ThreadCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство ThreadCount указывает число активных потоков в процессе. Инструкция — это элементарная единица выполняемых компьютером действий, а поток — это объект, который занят выполнением инструкций. Каждый выполняемый процесс имеет хотя бы один поток. Это свойство используется только для компьютеров под управлением Windows NT.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ThreadCount {
            get {
                if ((curObj["ThreadCount"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ThreadCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsUserModeTimeNull {
            get {
                if ((curObj["UserModeTime"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Время в пользовательском режиме, в 100 нc. Если эта информация отсутствует, испол" +
            "ьзуется значение 0.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong UserModeTime {
            get {
                if ((curObj["UserModeTime"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["UserModeTime"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVirtualSizeNull {
            get {
                if ((curObj["VirtualSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Свойство VirtualSize указывает текущий размер (в байтах) виртуального адресного пространства, используемого процессом. Использование виртуального адресного пространства не означает обязательного использования соответствующего пространства на диске или в оперативной памяти. Виртуальное пространство имеет конечные размеры, и при чрезмерном использовании процесс может ограничить возможность загрузки библиотек.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong VirtualSize {
            get {
                if ((curObj["VirtualSize"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["VirtualSize"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство WindowsVersion указывает версию Windows, в которой выполняется процесс.\n" +
            "Пример: 4.0")]
        public string WindowsVersion {
            get {
                return ((string)(curObj["WindowsVersion"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWorkingSetSizeNull {
            get {
                if ((curObj["WorkingSetSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Размер памяти (в байтах), необходимый для эффективного выполнения процесса в операционной системе, использующей страничную организацию памяти. Если памяти недостаточно, происходит пробуксовка. Если эта информация неизвестна, нужно ввести значение NULL или 0.  Если эти данные указаны, за ними можно вести наблюдение, чтобы понять, как меняются требования к памяти по мере выполнения процесса.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong WorkingSetSize {
            get {
                if ((curObj["WorkingSetSize"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["WorkingSetSize"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWriteOperationCountNull {
            get {
                if ((curObj["WriteOperationCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство WriteOperationCount указывает число выполненных операций записи.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong WriteOperationCount {
            get {
                if ((curObj["WriteOperationCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["WriteOperationCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWriteTransferCountNull {
            get {
                if ((curObj["WriteTransferCount"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Свойство WriteTransferCount указывает размер записанных данных.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ulong WriteTransferCount {
            get {
                if ((curObj["WriteTransferCount"] == null)) {
                    return System.Convert.ToUInt64(0);
                }
                return ((ulong)(curObj["WriteTransferCount"]));
            }
        }

        private bool CheckIfProperClass(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions OptionsParam) {
            if (((path != null)
                        && (string.Compare(path.ClassName, this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                return CheckIfProperClass(new System.Management.ManagementObject(mgmtScope, path, OptionsParam));
            }
        }

        private bool CheckIfProperClass(System.Management.ManagementBaseObject theObj) {
            if (((theObj != null)
                        && (string.Compare(((string)(theObj["__CLASS"])), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                System.Array parentClasses = ((System.Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null)) {
                    int count = 0;
                    for (count = 0; (count < parentClasses.Length); count = (count + 1)) {
                        if ((string.Compare(((string)(parentClasses.GetValue(count))), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Преобразует данные дату и время в формате DMTF в объект System.DateTime.
        static System.DateTime ToDateTime(string dmtfDate) {
            System.DateTime initializer = System.DateTime.MinValue;
            int year = initializer.Year;
            int month = initializer.Month;
            int day = initializer.Day;
            int hour = initializer.Hour;
            int minute = initializer.Minute;
            int second = initializer.Second;
            long ticks = 0;
            string dmtf = dmtfDate;
            System.DateTime datetime = System.DateTime.MinValue;
            string tempString = string.Empty;
            if ((dmtf == null)) {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((dmtf.Length == 0)) {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((dmtf.Length != 25)) {
                throw new System.ArgumentOutOfRangeException();
            }
            try {
                tempString = dmtf.Substring(0, 4);
                if (("****" != tempString)) {
                    year = int.Parse(tempString);
                }
                tempString = dmtf.Substring(4, 2);
                if (("**" != tempString)) {
                    month = int.Parse(tempString);
                }
                tempString = dmtf.Substring(6, 2);
                if (("**" != tempString)) {
                    day = int.Parse(tempString);
                }
                tempString = dmtf.Substring(8, 2);
                if (("**" != tempString)) {
                    hour = int.Parse(tempString);
                }
                tempString = dmtf.Substring(10, 2);
                if (("**" != tempString)) {
                    minute = int.Parse(tempString);
                }
                tempString = dmtf.Substring(12, 2);
                if (("**" != tempString)) {
                    second = int.Parse(tempString);
                }
                tempString = dmtf.Substring(15, 6);
                if (("******" != tempString)) {
                    ticks = (long.Parse(tempString) * ((long)((System.TimeSpan.TicksPerMillisecond / 1000))));
                }
                if (((((((((year < 0)
                            || (month < 0))
                            || (day < 0))
                            || (hour < 0))
                            || (minute < 0))
                            || (minute < 0))
                            || (second < 0))
                            || (ticks < 0))) {
                    throw new System.ArgumentOutOfRangeException();
                }
            }
            catch (System.Exception e) {
                throw new System.ArgumentOutOfRangeException(null, e.Message);
            }
            datetime = new System.DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            int UTCOffset = 0;
            int OffsetToBeAdjusted = 0;
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            tempString = dmtf.Substring(22, 3);
            if ((tempString != "******")) {
                tempString = dmtf.Substring(21, 4);
                try {
                    UTCOffset = int.Parse(tempString);
                }
                catch (System.Exception e) {
                    throw new System.ArgumentOutOfRangeException(null, e.Message);
                }
                OffsetToBeAdjusted = ((int)((OffsetMins - UTCOffset)));
                datetime = datetime.AddMinutes(((double)(OffsetToBeAdjusted)));
            }
            return datetime;
        }

        // Преобразует данный объект System.DateTime в формат даты и времени DMTF.
        static string ToDmtfDateTime(System.DateTime date) {
            string utcString = string.Empty;
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(date);
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            if ((System.Math.Abs(OffsetMins) > 999)) {
                date = date.ToUniversalTime();
                utcString = "+000";
            }
            else {
                if ((tickOffset.Ticks >= 0)) {
                    utcString = string.Concat("+", ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute))).ToString().PadLeft(3, '0'));
                }
                else {
                    string strTemp = ((long)(OffsetMins)).ToString();
                    utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
                }
            }
            string dmtfDateTime = ((int)(date.Year)).ToString().PadLeft(4, '0');
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Month)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Day)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Hour)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Minute)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Second)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ".");
            System.DateTime dtTemp = new System.DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            long microsec = ((long)((((date.Ticks - dtTemp.Ticks)
                        * 1000)
                        / System.TimeSpan.TicksPerMillisecond)));
            string strMicrosec = ((long)(microsec)).ToString();
            if ((strMicrosec.Length > 6)) {
                strMicrosec = strMicrosec.Substring(0, 6);
            }
            dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, utcString);
            return dmtfDateTime;
        }

        private bool ShouldSerializeCreationDate() {
            if ((this.IsCreationDateNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeExecutionState() {
            if ((this.IsExecutionStateNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeHandleCount() {
            if ((this.IsHandleCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeInstallDate() {
            if ((this.IsInstallDateNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeKernelModeTime() {
            if ((this.IsKernelModeTimeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeMaximumWorkingSetSize() {
            if ((this.IsMaximumWorkingSetSizeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeMinimumWorkingSetSize() {
            if ((this.IsMinimumWorkingSetSizeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeOtherOperationCount() {
            if ((this.IsOtherOperationCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeOtherTransferCount() {
            if ((this.IsOtherTransferCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePageFaults() {
            if ((this.IsPageFaultsNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePageFileUsage() {
            if ((this.IsPageFileUsageNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeParentProcessId() {
            if ((this.IsParentProcessIdNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePeakPageFileUsage() {
            if ((this.IsPeakPageFileUsageNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePeakVirtualSize() {
            if ((this.IsPeakVirtualSizeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePeakWorkingSetSize() {
            if ((this.IsPeakWorkingSetSizeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePriority() {
            if ((this.IsPriorityNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePrivatePageCount() {
            if ((this.IsPrivatePageCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeProcessId() {
            if ((this.IsProcessIdNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeQuotaNonPagedPoolUsage() {
            if ((this.IsQuotaNonPagedPoolUsageNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeQuotaPagedPoolUsage() {
            if ((this.IsQuotaPagedPoolUsageNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeQuotaPeakNonPagedPoolUsage() {
            if ((this.IsQuotaPeakNonPagedPoolUsageNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeQuotaPeakPagedPoolUsage() {
            if ((this.IsQuotaPeakPagedPoolUsageNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeReadOperationCount() {
            if ((this.IsReadOperationCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeReadTransferCount() {
            if ((this.IsReadTransferCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeSessionId() {
            if ((this.IsSessionIdNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeTerminationDate() {
            if ((this.IsTerminationDateNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeThreadCount() {
            if ((this.IsThreadCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeUserModeTime() {
            if ((this.IsUserModeTimeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeVirtualSize() {
            if ((this.IsVirtualSizeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeWorkingSetSize() {
            if ((this.IsWorkingSetSizeNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeWriteOperationCount() {
            if ((this.IsWriteOperationCountNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeWriteTransferCount() {
            if ((this.IsWriteTransferCountNull == false)) {
                return true;
            }
            return false;
        }

        [Browsable(true)]
        public void CommitObject() {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put();
            }
        }

        [Browsable(true)]
        public void CommitObject(System.Management.PutOptions putOptions) {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put(putOptions);
            }
        }

        private void Initialize() {
            AutoCommitProp = true;
            isEmbedded = false;
        }

        private static string ConstructPath(string keyHandle) {
            string strPath = "root\\cimv2:Win32_Process";
            strPath = string.Concat(strPath, string.Concat(".Handle=", string.Concat("\"", string.Concat(keyHandle, "\""))));
            return strPath;
        }

        private void InitializeObject(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            Initialize();
            if ((path != null)) {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true)) {
                    throw new System.ArgumentException("Имя класса не совпадает.");
                }
            }
            PrivateLateBoundObject = new System.Management.ManagementObject(mgmtScope, path, getOptions);
            PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
            curObj = PrivateLateBoundObject;
        }

        // Различные перезагрузки GetInstances(), которые помогают в перечислении экземпляров класса WMI.
        public static ProcessCollection GetInstances() {
            return GetInstances(null, null, null);
        }

        public static ProcessCollection GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }

        public static ProcessCollection GetInstances(string[] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }

        public static ProcessCollection GetInstances(string condition, string[] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }

        public static ProcessCollection GetInstances(System.Management.ManagementScope mgmtScope, System.Management.EnumerationOptions enumOptions) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\cimv2";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementPath pathObj = new System.Management.ManagementPath();
            pathObj.ClassName = "Win32_Process";
            pathObj.NamespacePath = "root\\cimv2";
            System.Management.ManagementClass clsObject = new System.Management.ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null)) {
                enumOptions = new System.Management.EnumerationOptions();
                enumOptions.EnsureLocatable = true;
            }
            return new ProcessCollection(clsObject.GetInstances(enumOptions));
        }

        public static ProcessCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }

        public static ProcessCollection GetInstances(System.Management.ManagementScope mgmtScope, string[] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }

        public static ProcessCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition, string[] selectedProperties) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\cimv2";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementObjectSearcher ObjectSearcher = new System.Management.ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_Process", condition, selectedProperties));
            System.Management.EnumerationOptions enumOptions = new System.Management.EnumerationOptions();
            enumOptions.EnsureLocatable = true;
            ObjectSearcher.Options = enumOptions;
            return new ProcessCollection(ObjectSearcher.Get());
        }

        [Browsable(true)]
        public static WIN32_Process CreateInstance() {
            System.Management.ManagementScope mgmtScope = null;
            if ((statMgmtScope == null)) {
                mgmtScope = new System.Management.ManagementScope();
                mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
            }
            else {
                mgmtScope = statMgmtScope;
            }
            System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
            System.Management.ManagementClass tmpMgmtClass = new System.Management.ManagementClass(mgmtScope, mgmtPath, null);
            return new WIN32_Process(tmpMgmtClass.CreateInstance());
        }

        [Browsable(true)]
        public void Delete() {
            PrivateLateBoundObject.Delete();
        }

        public uint AttachDebugger() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("AttachDebugger", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }

        public static uint Create(string CommandLine, string CurrentDirectory, System.Management.ManagementBaseObject ProcessStartupInformation, out uint ProcessId) {
            bool IsMethodStatic = true;
            if ((IsMethodStatic == true)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
                System.Management.ManagementClass classObj = new System.Management.ManagementClass(statMgmtScope, mgmtPath, null);
                bool EnablePrivileges = classObj.Scope.Options.EnablePrivileges;
                classObj.Scope.Options.EnablePrivileges = true;
                inParams = classObj.GetMethodParameters("Create");
                inParams["CommandLine"] = ((string)(CommandLine));
                inParams["CurrentDirectory"] = ((string)(CurrentDirectory));
                inParams["ProcessStartupInformation"] = ((System.Management.ManagementBaseObject )(ProcessStartupInformation));
                System.Management.ManagementBaseObject outParams = classObj.InvokeMethod("Create", inParams, null);
                ProcessId = System.Convert.ToUInt32(outParams.Properties["ProcessId"].Value);
                classObj.Scope.Options.EnablePrivileges = EnablePrivileges;
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                ProcessId = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }

        public uint GetAvailableVirtualSize(out ulong AvailableVirtualSize) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                bool EnablePrivileges = PrivateLateBoundObject.Scope.Options.EnablePrivileges;
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = true;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetAvailableVirtualSize", inParams, null);
                AvailableVirtualSize = System.Convert.ToUInt64(outParams.Properties["AvailableVirtualSize"].Value);
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = EnablePrivileges;
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                AvailableVirtualSize = System.Convert.ToUInt64(0);
                return System.Convert.ToUInt32(0);
            }
        }

        public uint GetOwner(out string Domain, out string User) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                bool EnablePrivileges = PrivateLateBoundObject.Scope.Options.EnablePrivileges;
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = true;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetOwner", inParams, null);
                Domain = System.Convert.ToString(outParams.Properties["Domain"].Value);
                User = System.Convert.ToString(outParams.Properties["User"].Value);
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = EnablePrivileges;
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                Domain = null;
                User = null;
                return System.Convert.ToUInt32(0);
            }
        }

        public uint GetOwnerSid(out string Sid) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                bool EnablePrivileges = PrivateLateBoundObject.Scope.Options.EnablePrivileges;
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = true;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetOwnerSid", inParams, null);
                Sid = System.Convert.ToString(outParams.Properties["Sid"].Value);
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = EnablePrivileges;
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                Sid = null;
                return System.Convert.ToUInt32(0);
            }
        }

        public uint SetPriority(int Priority) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                bool EnablePrivileges = PrivateLateBoundObject.Scope.Options.EnablePrivileges;
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = true;
                inParams = PrivateLateBoundObject.GetMethodParameters("SetPriority");
                inParams["Priority"] = ((int)(Priority));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("SetPriority", inParams, null);
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = EnablePrivileges;
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }

        public uint Terminate(uint Reason) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                bool EnablePrivileges = PrivateLateBoundObject.Scope.Options.EnablePrivileges;
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = true;
                inParams = PrivateLateBoundObject.GetMethodParameters("Terminate");
                inParams["Reason"] = ((uint)(Reason));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Terminate", inParams, null);
                PrivateLateBoundObject.Scope.Options.EnablePrivileges = EnablePrivileges;
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }

        public enum ExecutionStateValues {

            Неизвестно = 0,

            Другое = 1,

            Готов = 2,

            Выполняется = 3,

            Заблокирован = 4,

            Приостановлен_заблокирован = 5,

            Приостановлен_готов = 6,

            Прекращен = 7,

            Остановлен = 8,

            Рост = 9,

            NULL_ENUM_VALUE = 10,
        }

        // Реализация перечислителя для перечисления экземпляров класса.
        public class ProcessCollection : object, ICollection {

            private ManagementObjectCollection privColObj;

            public ProcessCollection(ManagementObjectCollection objCollection) {
                privColObj = objCollection;
            }

            public virtual int Count {
                get {
                    return privColObj.Count;
                }
            }

            public virtual bool IsSynchronized {
                get {
                    return privColObj.IsSynchronized;
                }
            }

            public virtual object SyncRoot {
                get {
                    return this;
                }
            }

            public virtual void CopyTo(System.Array array, int index) {
                privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1)) {
                    array.SetValue(new WIN32_Process(((System.Management.ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }

            public virtual System.Collections.IEnumerator GetEnumerator() {
                return new ProcessEnumerator(privColObj.GetEnumerator());
            }

            public class ProcessEnumerator : object, System.Collections.IEnumerator {

                private ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;

                public ProcessEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum) {
                    privObjEnum = objEnum;
                }

                public virtual object Current {
                    get {
                        return new WIN32_Process(((System.Management.ManagementObject)(privObjEnum.Current)));
                    }
                }

                public virtual bool MoveNext() {
                    return privObjEnum.MoveNext();
                }

                public virtual void Reset() {
                    privObjEnum.Reset();
                }
            }
        }

        // TypeConverter для обработки нулевых значений свойств ValueType
        public class WMIValueTypeConverter : TypeConverter {

            private TypeConverter baseConverter;

            private System.Type baseType;

            public WMIValueTypeConverter(System.Type inBaseType) {
                baseConverter = TypeDescriptor.GetConverter(inBaseType);
                baseType = inBaseType;
            }

            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type srcType) {
                return baseConverter.CanConvertFrom(context, srcType);
            }

            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType) {
                return baseConverter.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
                return baseConverter.ConvertFrom(context, culture, value);
            }

            public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary dictionary) {
                return baseConverter.CreateInstance(context, dictionary);
            }

            public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetCreateInstanceSupported(context);
            }

            public override PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributeVar) {
                return baseConverter.GetProperties(context, value, attributeVar);
            }

            public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetPropertiesSupported(context);
            }

            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValues(context);
            }

            public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesExclusive(context);
            }

            public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesSupported(context);
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) {
                if ((baseType.BaseType == typeof(System.Enum))) {
                    if ((value.GetType() == destinationType)) {
                        return value;
                    }
                    if ((((value == null)
                                && (context != null))
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return  "NULL_ENUM_VALUE" ;
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((baseType == typeof(bool))
                            && (baseType.BaseType == typeof(System.ValueType)))) {
                    if ((((value == null)
                                && (context != null))
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return "";
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((context != null)
                            && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                    return "";
                }
                return baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }

        // Встроенный класс, отражающий свойства системы WMI.
        [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        public class ManagementSystemProperties {

            private System.Management.ManagementBaseObject PrivateLateBoundObject;

            public ManagementSystemProperties(System.Management.ManagementBaseObject ManagedObject) {
                PrivateLateBoundObject = ManagedObject;
            }

            [Browsable(true)]
            public int GENUS {
                get {
                    return ((int)(PrivateLateBoundObject["__GENUS"]));
                }
            }

            [Browsable(true)]
            public string CLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__CLASS"]));
                }
            }

            [Browsable(true)]
            public string SUPERCLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__SUPERCLASS"]));
                }
            }

            [Browsable(true)]
            public string DYNASTY {
                get {
                    return ((string)(PrivateLateBoundObject["__DYNASTY"]));
                }
            }

            [Browsable(true)]
            public string RELPATH {
                get {
                    return ((string)(PrivateLateBoundObject["__RELPATH"]));
                }
            }

            [Browsable(true)]
            public int PROPERTY_COUNT {
                get {
                    return ((int)(PrivateLateBoundObject["__PROPERTY_COUNT"]));
                }
            }

            [Browsable(true)]
            public string[] DERIVATION {
                get {
                    return ((string[])(PrivateLateBoundObject["__DERIVATION"]));
                }
            }

            [Browsable(true)]
            public string SERVER {
                get {
                    return ((string)(PrivateLateBoundObject["__SERVER"]));
                }
            }

            [Browsable(true)]
            public string NAMESPACE {
                get {
                    return ((string)(PrivateLateBoundObject["__NAMESPACE"]));
                }
            }

            [Browsable(true)]
            public string PATH {
                get {
                    return ((string)(PrivateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}
